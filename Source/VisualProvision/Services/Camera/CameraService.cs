using System.Threading.Tasks;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using VisualProvision.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(CameraService))]
namespace VisualProvision.Services
{
    public class CameraService
    {
        public bool IsCameraAvailable => CrossMedia.Current.IsCameraAvailable;

        public bool IsTakePhotoSupported => CrossMedia.Current.IsTakePhotoSupported;

        public Task InitializeAsync()
        {
            return CrossMedia.Current.Initialize();
        }

        public async Task<MediaFile> GetMediaFileAsync()
        {
            var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                Directory = AppSettings.PhotoFolder,
                Name = "latest-whiteboard.jpg",
                SaveToAlbum = true,
                CompressionQuality = 80,
                PhotoSize = PhotoSize.MaxWidthHeight,
                MaxWidthHeight = 3000,
                DefaultCamera = CameraDevice.Rear,
            });

            if (file == null)
            {
                return null;
            }

            System.Diagnostics.Debug.WriteLine($"File Location: {file.Path}");

            return file;
        }

        public async Task<ImageSource> TakePhotoAsync()
        {
            var file = await GetMediaFileAsync();

            return ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                file.Dispose();
                return stream;
            });
        }

        public async Task<ImageSource> PickPhotoAsync()
        {
            var file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions() { PhotoSize = PhotoSize.Medium });

            return file == null ? null : ImageSource.FromStream(() =>
            {             
                var stream = file.GetStream();
                file.Dispose();
                return stream;
            });
        }

        public async Task<bool> CheckPermissionsAsync()
        {
            var cameraStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
            var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);
            var photosStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Photos);

            // Media plugin recommends to take care explicitly of permissions before trying to access the camera
            if (cameraStatus != PermissionStatus.Granted || 
                photosStatus != PermissionStatus.Granted ||
                storageStatus != PermissionStatus.Granted)
            {
                var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Camera, Permission.Photos, Permission.Storage });
                cameraStatus = results[Permission.Camera];
                photosStatus = results[Permission.Photos];
                storageStatus = results[Permission.Storage];
            }

            return cameraStatus == PermissionStatus.Granted && photosStatus == PermissionStatus.Granted && storageStatus == PermissionStatus.Granted;            
        }

        public async Task<bool> MustShowRationaleAsync()
        {
            var cameraStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
            var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);
            var photosStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Photos);

            if (cameraStatus != PermissionStatus.Granted ||
                photosStatus != PermissionStatus.Granted ||
                storageStatus != PermissionStatus.Granted)
            {
                return await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Camera);
            }
            else
            {
                return false;
            }
        }
    }
}
