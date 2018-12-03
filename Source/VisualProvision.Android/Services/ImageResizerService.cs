using System.IO;
using System.Threading.Tasks;
using Android.Graphics;
using VisualProvision.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(VisualProvision.Droid.Services.ImageResizerService))]
namespace VisualProvision.Droid.Services
{
    public class ImageResizerService : IImageResizerService
    {
        public Task<byte[]> ResizeImageAsync(byte[] imageData, int widthPixels, int heightPixels)
        {
            Bitmap originalImage = BitmapFactory.DecodeByteArray(imageData, 0, imageData.Length);
            Bitmap resizedImage = Bitmap.CreateScaledBitmap(originalImage, widthPixels, heightPixels, false);

            using (var ms = new MemoryStream())
            {
                resizedImage.Compress(Bitmap.CompressFormat.Jpeg, 80, ms);
                return Task.FromResult(ms.ToArray());
            }
        }

        public Task<byte[]> ResizeImageToHalfSizeAsync(byte[] imageData)
        {
            var bitmapOptions = new BitmapFactory.Options
            {
                InJustDecodeBounds = true,
            };
            Bitmap decodedBitmap = BitmapFactory.DecodeByteArray(imageData, 0, imageData.Length);

            int imageHeight = decodedBitmap.Height;
            int imageWidth = decodedBitmap.Width;

            return ResizeImageAsync(imageData, imageWidth / 2, imageHeight / 2);
        }
    }
}