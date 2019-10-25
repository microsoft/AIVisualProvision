using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using VisualProvision.Resources;
using VisualProvision.Services.Management;
using VisualProvision.Utils;
using VisualProvision.Utils.Validations;
using Xamarin.Forms;

using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

namespace VisualProvision.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private ValidatableObject<string> password;
        private ValidatableObject<string> clientId;
        private ValidatableObject<string> tenantId;

        private readonly SubscriptionsCache cache;
        private bool isValid;

        public LoginViewModel()
        {
            cache = DependencyService.Get<SubscriptionsCache>();

            string emptyValidationMessage = Translations.Validations_NullOrEmptyMessage;
            var emptyRule = new IsNotNullOrEmptyRule
            {
                ValidationMessage = emptyValidationMessage,
            };

            password = new ValidatableObject<string>();
            password.Validations.Add(emptyRule);

            clientId = new ValidatableObject<string>();
            clientId.Validations.Add(emptyRule);

            tenantId = new ValidatableObject<string>();
            tenantId.Validations.Add(emptyRule);

#if DEBUG
            /*
             * Avoid having to enter credentials every time app is deployed 
             * during development
             */

            ClientId.Value = AppSettings.ClientId;
            TenantId.Value = AppSettings.TenantId;

            ClientId.IsValid = TenantId.IsValid = Password.IsValid = true;
#else
            ClientId.IsValid = TenantId.IsValid = Password.IsValid = false;
#endif
        }

        public ValidatableObject<string> Password
        {
            get => password;

            set => SetProperty(ref password, value);
        }

        public ValidatableObject<string> TenantId
        {
            get => tenantId;

            set => SetProperty(ref tenantId, value);
        }

        public ValidatableObject<string> ClientId
        {
            get => clientId;

            set => SetProperty(ref clientId, value);
        }

        public bool IsValid
        {
            get => isValid;

            set => SetProperty(ref isValid, value);
        }

        public ICommand LoginCommand => new Command(async (o) => await LoginAsync());

        public override async Task NavigatedToAsync(object navigationParameter)
        {
            await base.NavigatedToAsync(navigationParameter);

            if (!await AppSettingsValidator.CheckSettingsAsync())
            {
                return;
            }

            password.ValueChanged += PasswordChanged;
            clientId.ValueChanged += ClientIdChanged;
            tenantId.ValueChanged += TenantIdChanged;
        }

        public override Task NavigatedFromAsync()
        {
            password.ValueChanged -= PasswordChanged;
            clientId.ValueChanged -= ClientIdChanged;
            tenantId.ValueChanged -= TenantIdChanged;

            return base.NavigatedFromAsync();
        }

        private async Task LoginAsync()
        {
            Crashes.GenerateTestCrash();

            bool loginSuccess = await TryAuthenticateAsync();

            if (loginSuccess)
            {
                await NavigationService.NavigateToAsync<SubscriptionViewModel>();
            }
        }

        private async Task<bool> TryAuthenticateAsync(CancellationToken ct = default(CancellationToken))
        {
            if (!await CheckConnectivityAsync())
            {
                return false;
            }

            IsBusy = true;

            try
            {
                await AzureService.SetCredentialsAsync(
                    ClientId.Value, 
                    TenantId.Value, 
                    password.Value, 
                    ct);

                if (!ct.IsCancellationRequested)
                {
                    var subscriptions = await AzureService.GetSubscriptionsAsync();
                    await cache.SaveAsync(subscriptions);
                }
            }
            catch (ServiceException serviceEx)
            {
                System.Diagnostics.Debug.WriteLine($"Azure service exception: {serviceEx}");

                if (!ct.IsCancellationRequested)
                {
                    await DisplayAlert(
                        Translations.Dialog_ServiceError_Title, 
                        Translations.Dialog_ServiceError_Message);
                }

                return false;
            }
            catch (AuthenticationException authEx)
            {
                System.Diagnostics.Debug.WriteLine($"Invalid password: {authEx}");

                if (!ct.IsCancellationRequested)
                {
                    await DisplayAlert(
                        Translations.Dialog_CredentialsError_Title, 
                        Translations.Dialog_CredentialsError_Message);
                }

                return false;
            }
            finally
            {
                IsBusy = false;
            }

            return true;
        }

        private void PasswordChanged(object sender, string e)
        {
            IsValid = Password.Validate() && ClientId.IsValid && TenantId.IsValid;
        }

        private void ClientIdChanged(object sender, string e)
        {
            IsValid = ClientId.Validate() && Password.IsValid && TenantId.IsValid;
        }

        private void TenantIdChanged(object sender, string e)
        {
            IsValid = TenantId.Validate() && Password.IsValid && ClientId.IsValid;
        }
    }
}