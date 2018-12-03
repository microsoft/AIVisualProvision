using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Plugin.Media.Abstractions;
using VisualProvision.Models;
using VisualProvision.Resources;
using VisualProvision.Services;
using VisualProvision.Services.Management;
using VisualProvision.Services.Recognition;
using Xamarin.Forms;

namespace VisualProvision.ViewModels
{
    public class ServicesViewModel : BaseViewModel
    {   
        private string statusMessage;
        private bool resourcesDetectionSuceeded;
        private bool canContinue;
        private List<AzureResourceModel> azureResources;
        private readonly IRecognitionService recognitionService;

        public ServicesViewModel()
        {
            recognitionService = DependencyService.Get<IRecognitionService>();
        }

        public string StatusMessage
        {
            get => statusMessage;

            set => SetProperty(ref statusMessage, value);
        }

        public bool ResourcesDetectionSuceeded
        {
            get => resourcesDetectionSuceeded;

            set => SetProperty(ref resourcesDetectionSuceeded, value);
        }

        public bool CanContinue
        {
            get => canContinue;

            set => SetProperty(ref canContinue, value);
        }

        public List<AzureResourceModel> AzureResources
        {
            get => azureResources;

            set => SetProperty(ref azureResources, value);
        }

        private MediaFile CapturedImage { get; set; }

        public ICommand BackCommand => new Command(async (o) => await GoBackAsync());

        public ICommand NavigateToResourceGroupCommand => new Command(async (o) => await NavigateToResourceGroupAsync());

        public override async Task NavigatedToAsync(object navigationParameter)
        {
            IsBusy = true;

            await base.NavigatedToAsync(navigationParameter);

            if (navigationParameter != null && navigationParameter is MediaFile)
            {
                CapturedImage = navigationParameter as MediaFile;
            }
            else
            {
                // Error in received parameter
                await DisplayAlert(
                    Translations.Dialog_PhotoFailed_Title,
                    Translations.Dialog_PhotoFailed_Message);
                await NavigationService.NavigateBackAsync();
                return;
            }       

            await DetectResourcesAsync();
            await NavigationService.RemovePreviousAsync<MediaViewModel>();

            IsBusy = false;
        }

        public override Task UnloadAsync()
        {
            CapturedImage = null;
            return base.UnloadAsync();           
        }

        private async Task DetectResourcesAsync()
        {
            if (!await CheckConnectivityAsync())
            {
                return;
            }

            StatusMessage = Translations.Services_Recognition_Working;

            IEnumerable<AzureResource> detectedResources = 
                await recognitionService.GetResourcesAsync(CapturedImage);

            AzureResources = detectedResources?
                .Select(AzureResourceModel.FromResource)
                .ToList();

            AzureResourceManager.Instance.ResourcesToDeploy =
                AzureResourceManager.Instance.AvailableResources?
                    .Where(r => AzureResources?.Any(az => az.Type == r.Type) == true)
                    .ToList();

            CanContinue = AzureResources.Any();

            switch (AzureResources.Count)
            {
                case 0:
                    StatusMessage = Translations.Services_Recognition_NotFound;
                    break;
                case 1:
                    StatusMessage = Translations.Services_Recognition_OneFound;
                    break;
                default:
                    StatusMessage = string.Format(
                        Translations.Services_Recognition_ManyFound, 
                        AzureResources.Count);
                    break;
            }

            ResourcesDetectionSuceeded = true;
        }

        private Task GoBackAsync()
        {
            return NavigationService.NavigateBackAsync();
        }

        private async Task NavigateToResourceGroupAsync()
        {
            await NavigationService.NavigateToAsync<ResourceGroupViewModel>();
        }
    }
}
