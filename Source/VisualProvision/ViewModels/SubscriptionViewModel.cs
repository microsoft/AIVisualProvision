using VisualProvision.Models;
using VisualProvision.Services.Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace VisualProvision.ViewModels
{
    public class SubscriptionViewModel : BaseViewModel
    {
        private bool canContinue;

        private readonly SubscriptionsCache cache;
        private IEnumerable<Subscription> subscriptions;
        private Subscription currentSubscription;

        public SubscriptionViewModel()
        {
            cache = DependencyService.Get<SubscriptionsCache>();
        }

        public bool CanContinue
        {
            get => canContinue;

            set => SetProperty(ref canContinue, value);
        }

        public IEnumerable<Subscription> Subscriptions
        {
            get => subscriptions;

            set => SetProperty(ref subscriptions, value);
        }

        public Subscription CurrentSubscription
        {
            get => currentSubscription;

            set
            {
                if (SetProperty(ref currentSubscription, value))
                {
                    UpdateCanContinue();
                }
            }
        }

        public ICommand RefreshCommand => new Command(async (o) => await RefreshAsync());

        public ICommand ContinueCommand => new Command(async (o) => await ContinueAsync());

        public override async Task NavigatedToAsync(object navigationParameter)
        {
            IsBusy = true;

            await base.NavigatedToAsync(navigationParameter);
            await LoadSubscriptionsAsync(true);

            IsBusy = false;
        }

        public override async Task NavigatedBackAsync()
        {
            IsBusy = true;

            await LoadSubscriptionsAsync(true);

            IsBusy = false;
        }

        public override async Task NavigatedFromAsync()
        {
            await base.NavigatedFromAsync();

            if (await AzureService.HasSavedCredentialsAsync())
            {
                await AzureService.EnsureAuthenticateAsync();

                if (Subscriptions?.Any() == true)
                {
                    await cache.SaveAsync(Subscriptions);
                }

                if (CurrentSubscription != null)
                {
                    await cache.SaveCurrentAsync(CurrentSubscription.SubscriptionId);
                }

                await LoadSubscriptionsAsync(true);
            }
        }

        private async Task ContinueAsync()
        {
            if (CanContinue)
            {
                await NavigationService.NavigateToAsync<MediaViewModel>();
            }
        }

        private async Task LoadSubscriptionsAsync(bool fromCache)
        {
            if (!await CheckConnectivityAsync())
            {
                return;
            }

            IEnumerable<Subscription> allSubscriptions = fromCache
                ? await cache.GetAsync()
                : await AzureService.GetSubscriptionsAsync();

            Subscriptions = allSubscriptions?.ToList();

            // Give some time to bindings to update
            await Task.Delay(50);
            await LoadDefaultSubscriptionAsync();

            UpdateCanContinue();
        }

        private async Task LoadDefaultSubscriptionAsync()
        {
            string currentSubscriptionId = await cache.GetCurrentAsync();

            if (!string.IsNullOrEmpty(currentSubscriptionId))
            {
                CurrentSubscription = Subscriptions?.FirstOrDefault(s => s.SubscriptionId == currentSubscriptionId);
            }
        }

        private async Task RefreshAsync()
        {
            IsBusy = true;

            await LoadSubscriptionsAsync(false);
            await cache.SaveAsync(Subscriptions);

            IsBusy = false;
        }

        private void UpdateCanContinue()
        {
            CanContinue = CurrentSubscription != null;
        }
    }
}