using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Management.Fluent;
using VisualProvision.Models;
using VisualProvision.Services.Management.Deployment;

namespace VisualProvision.Services.Management
{
    public interface IAzureService
    {
        Azure.IAuthenticated Authenticated { get; }

        DeploymentOptions DeploymentOptions { get; set; }

        Task<string> GetSavedClientIdAsync();

        Task<string> GetSavedTenantIdAsync();

        Task<string> GetSavedPasswordAsync();

        Task<bool> HasSavedCredentialsAsync();

        Task ClearAllSavedDataAsync();

        Task SetCredentialsAsync(
            string clientId, 
            string tenantId, 
            string password, 
            CancellationToken ct = default(CancellationToken));

        Task EnsureAuthenticateAsync();

        Task<IEnumerable<Subscription>> GetSubscriptionsAsync();

        Task<IEnumerable<ResourceGroup>> GetResourceGroupsAsync();

        Task<IEnumerable<ResourceGroup>> GetResourceGroupsBySubscriptionAsync(string subscriptionId);

        Task<IEnumerable<Ubication>> GetRegionsAsync();
    }
}
