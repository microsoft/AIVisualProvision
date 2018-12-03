using VisualProvision.Models;
using VisualProvision.Services.Management;
using VisualProvision.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: Dependency(typeof(SubscriptionsCache))]
namespace VisualProvision.Services.Management
{
    public class SubscriptionsCache
    {
        private const string CurrentSubscriptionKey = "current-subscription";
        private const string SubscriptionsKey = "subscriptions";

        public Task<string> GetCurrentAsync()
        {
            return SecureStorage.GetAsync(CurrentSubscriptionKey);
        }

        public Task<IEnumerable<Subscription>> GetAsync()
        {
            return SecureStorageHelper.GetObjectAsync<IEnumerable<Subscription>>(SubscriptionsKey);
        }

        public Task SaveCurrentAsync(string subscriptionId)
        {
            return SecureStorage.SetAsync(CurrentSubscriptionKey, subscriptionId);
        }

        public Task SaveAsync(IEnumerable<Subscription> subscriptions)
        {
            return SecureStorageHelper.SetObjectAsync(SubscriptionsKey, subscriptions);
        }

        public Task ClearAsync()
        {
            SecureStorage.Remove(CurrentSubscriptionKey);
            SecureStorage.Remove(SubscriptionsKey);

            return Task.CompletedTask;
        }
    }
}
