using System.Threading.Tasks;
using Plugin.Media.Abstractions;
using VisualProvision.Resources;
using VisualProvision.Services;
using Xamarin.Forms;

namespace VisualProvision.ViewModels
{
    public class MediaViewModel : BaseViewModel
    {
        private readonly CameraService cameraService;

        public MediaViewModel()
        {
            cameraService = DependencyService.Resolve<CameraService>();
        }

        public MediaFile CurrentMediaFile { get; set; }

        public override async Task NavigatedToAsync(object navigationParameter)
        {
            await base.NavigatedToAsync(navigationParameter);
            await cameraService.InitializeAsync();
            bool successTakingPhoto = await TakePhotoAsync();

            if (successTakingPhoto)
            {
                await NavigateToServicesAsync(CurrentMediaFile);
            }
            else
            {
                // Navigate back. Dialog is already shown
                await NavigationService.NavigateBackAsync();
            }            
        }

        private async Task<bool> TakePhotoAsync()
        {
            if (await cameraService.MustShowRationaleAsync())
            {
                await DisplayAlert(
                    Translations.Dialog_RequiredPermissions_Title, 
                    Translations.Dialog_RequiredPermissions_Message);
            }

            bool hasPermissions = await cameraService.CheckPermissionsAsync();
            if (!hasPermissions)
            {
                await DisplayAlert(
                    Translations.Dialog_CheckPermissions_Title,
                    Translations.Dialog_CheckPermissions_Message);
                return false;
            }

            if (!cameraService.IsCameraAvailable || !cameraService.IsTakePhotoSupported)
            {
                await DisplayAlert(
                    Translations.Dialog_CameraNotAvailable_Title,
                    Translations.Dialog_CameraNotAvailable_Message);
                return false;
            }

            CurrentMediaFile = await cameraService.GetMediaFileAsync();
            return true;
        }

        private async Task NavigateToServicesAsync(MediaFile capturedImage)
        {
            await NavigationService.NavigateToAsync<ServicesViewModel>(capturedImage);
        }
    }
}