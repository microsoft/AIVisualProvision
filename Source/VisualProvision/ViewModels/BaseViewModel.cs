using System.Threading.Tasks;
using System.Windows.Input;
using VisualProvision.Common;
using VisualProvision.Resources;
using VisualProvision.Services;
using VisualProvision.Services.Connectivity;
using VisualProvision.Services.Dialog;
using VisualProvision.Services.Management;
using Xamarin.Forms;

namespace VisualProvision.ViewModels
{
    public abstract class BaseViewModel : BindableBase
    {
        private bool isBusy;
        private bool hasNetworkAccess;

        protected BaseViewModel()
        {
            AzureService = DependencyService.Resolve<IAzureService>(DependencyFetchTarget.GlobalInstance);
            ConnectivityService = DependencyService.Resolve<ConnectivityService>();
            DialogService = DependencyService.Resolve<DialogService>();
            NavigationService = DependencyService.Resolve<NavigationService>();
        }

        public IAzureService AzureService { get; private set; }

        public ConnectivityService ConnectivityService { get; private set; }

        public DialogService DialogService { get; private set; }

        public NavigationService NavigationService { get; private set; }

        public bool IsBusy
        {
            get => isBusy;

            set => SetProperty(ref this.isBusy, value);
        }

        public bool HasNetworkAccess
        {
            get => hasNetworkAccess;

            set => SetProperty(ref this.hasNetworkAccess, value);
        }

        public ICommand LogoutCommand => new Command(async (o) => await LogoutAsync());

        public virtual Task NavigatedToAsync(object navigationParameter)
        {
            HasNetworkAccess = ConnectivityService.HasNetworkAccess;

            return Task.CompletedTask;
        }

        public virtual Task NavigatedFromAsync()
        {
            return Task.CompletedTask;
        }

        public virtual Task NavigatedBackAsync()
        {
            return Task.CompletedTask;
        }

        public virtual Task UnloadAsync()
        {
            return Task.CompletedTask;
        }

        protected Task DisplayAlert(string title, string message)
        {
            return DialogService.DisplayAlert(title, message);
        }

        protected async Task<bool> CheckConnectivityAsync()
        {
            if (!HasNetworkAccess)
            {
                await DisplayAlert(Translations.Dialog_Connectivity_Title, Translations.Dialog_Connectivity_Message);
            }

            return HasNetworkAccess;
        }

        private async Task LogoutAsync()
        {
            string title = Translations.Dialog_Logout_Title;
            string message = Translations.Dialog_Logout_Message;

            if (await DialogService.DisplayConfirmation(title, message))
            {
                // Clear saved variables
                await AzureService.ClearAllSavedDataAsync();
                await NavigationService.ClearNavigateToAsync<LoginViewModel>();
            }
        }
    }
}