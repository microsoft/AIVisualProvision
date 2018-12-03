using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using VisualProvision.Models;
using VisualProvision.Services.Management.Deployment;
using Xamarin.Essentials;
using Xamarin.Forms;
using Region = Microsoft.Azure.Management.ResourceManager.Fluent.Core.Region;

namespace VisualProvision.Services.Management
{
    public class AzureService : IAzureService
    {
        private const string ClientIdKey = "clientId";
        private const string TenantIdKey = "tenantId";
        private const string PasswordKey = "password";

        private readonly SubscriptionsCache subscriptionsCache;

        public AzureService()
        {
            subscriptionsCache = DependencyService.Get<SubscriptionsCache>();
        }

        public Azure.IAuthenticated Authenticated { get; private set; }

        public DeploymentOptions DeploymentOptions { get; set; } = DeploymentOptions.Default;

        public Task<string> GetSavedClientIdAsync()
        {
            return Task.FromResult(Preferences.Get(ClientIdKey, string.Empty));
        }

        public Task<string> GetSavedTenantIdAsync()
        {
            return Task.FromResult(Preferences.Get(TenantIdKey, string.Empty));
        }

        public Task<string> GetSavedPasswordAsync()
        {
            return SecureStorage.GetAsync(PasswordKey);
        }

        public async Task<bool> HasSavedCredentialsAsync()
        {
            string pwd = await GetSavedPasswordAsync();
            string clientId = await GetSavedClientIdAsync();
            string tenantId = await GetSavedTenantIdAsync();

            return !string.IsNullOrEmpty(pwd) &&
                !string.IsNullOrEmpty(clientId) &&
                !string.IsNullOrEmpty(tenantId);
        }

        public async Task ClearAllSavedDataAsync()
        {
            Preferences.Remove(ClientIdKey);
            Preferences.Remove(TenantIdKey);
            SecureStorage.Remove(PasswordKey);
            await subscriptionsCache.ClearAsync();
        }

        public Task SetCredentialsAsync(string clientId, string tenantId, string password, CancellationToken ct = default(CancellationToken))
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

            Task.Factory.StartNew(
                async () =>
                {
                    try
                    {
                        await AuthenticateAsync(clientId, tenantId, password, checkCredentials: true);             

                        Preferences.Set(ClientIdKey, clientId);
                        Preferences.Set(TenantIdKey, tenantId);
                        await SecureStorage.SetAsync(PasswordKey, password);

                        tcs.TrySetResult(true);
                    }
                    catch (Exception ex)
                    {
                        tcs.TrySetException(ex);
                    }
                }, ct);

            return tcs.Task;
        }

        public Task EnsureAuthenticateAsync()
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

            Task.Factory.StartNew(async () =>
            {
                try
                {
                    if (Authenticated == null)
                    {
                        string clientId = await GetSavedClientIdAsync();
                        string tenantId = await GetSavedTenantIdAsync();
                        string pwd = await GetSavedPasswordAsync();
                        await AuthenticateAsync(clientId, tenantId, pwd, checkCredentials: false);
                    }

                    tcs.TrySetResult(true);
                }
                catch (Exception ex)
                {
                    tcs.TrySetException(ex);
                }
            });

            return tcs.Task;
        }

        public virtual async Task<IEnumerable<Subscription>> GetSubscriptionsAsync()
        {
            if (Authenticated == null)
            {
                return Enumerable.Empty<Subscription>();
            }

            var subscriptionsList = await Authenticated.Subscriptions.ListAsync().ConfigureAwait(false);

            return subscriptionsList.Select(s => new Subscription
            {
                DisplayName = s.DisplayName,
                SubscriptionId = s.SubscriptionId,
            });
        }

        public async Task<IEnumerable<ResourceGroup>> GetResourceGroupsAsync()
        {
            string subscriptionId = await subscriptionsCache.GetCurrentAsync();

            return await GetResourceGroupsBySubscriptionAsync(subscriptionId);
        }

        public virtual Task<IEnumerable<ResourceGroup>> GetResourceGroupsBySubscriptionAsync(string subscriptionId)
        {
            var groups = Authenticated
                .WithSubscription(subscriptionId)
                .ResourceGroups
                .List();

            var result = groups.Select(g => new ResourceGroup
            {
                Name = g.Name,
            });

            return Task.FromResult(result);
        }

        public Task<IEnumerable<Ubication>> GetRegionsAsync()
        {
            var regions = new List<Ubication> 
            { 
                new Ubication { Name = "US East", Region = Region.USEast },
                new Ubication { Name = "US West", Region = Region.USWest },
                new Ubication { Name = "Europe West", Region = Region.EuropeWest },
                new Ubication { Name = "Asia East", Region = Region.AsiaEast },
            };

            return Task.FromResult(regions.AsEnumerable());
        }

        protected virtual async Task AuthenticateAsync(
            string clientId, 
            string tenantId, 
            string password, 
            bool checkCredentials = false)
        {
            AzureCredentials credentials = SdkContext
                       .AzureCredentialsFactory
                       .FromServicePrincipal(
                           clientId,
                           password,
                           tenantId,
                           AzureEnvironment.AzureGlobalCloud);

            Authenticated = Azure
                .Configure()
                .WithLogLevel(HttpLoggingDelegatingHandler.Level.Basic)
                .Authenticate(credentials);

            if (checkCredentials)
            {
                await ExecuteAsync(GetSubscriptionsAsync).ConfigureAwait(false);
            }
        }

        private async Task<T> ExecuteAsync<T>(Func<Task<T>> func)
        {
            T result = default(T);

            try
            {
                result = await func().ConfigureAwait(false);
            }
            catch (AdalServiceException serviceException) when (serviceException.StatusCode == (int)HttpStatusCode.BadRequest)
            {
                throw new ServiceException(serviceException.Message, serviceException);
            }
            catch (AdalServiceException serviceException) when (serviceException.StatusCode == (int)HttpStatusCode.Unauthorized)
            {
                throw new AuthenticationException(serviceException.Message, serviceException);
            }

            return result;
        }
    }
}