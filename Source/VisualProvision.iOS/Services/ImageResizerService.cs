using System;
using System.Drawing;
using System.Threading.Tasks;
using CoreGraphics;
using UIKit;
using VisualProvision.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(VisualProvision.iOS.Services.ImageResizerService))]
namespace VisualProvision.iOS.Services
{
    public class ImageResizerService : IImageResizerService
    {
        public Task<byte[]> ResizeImageAsync(byte[] imageData, int widthPixels, int heightPixels)
        {
            UIImage originalImage = ImageFromByteArray(imageData);
            UIImageOrientation orientation = originalImage.Orientation;

            // create a 24bit RGB image
            using (CGBitmapContext context = new CGBitmapContext(
                IntPtr.Zero,
                widthPixels, 
                heightPixels, 
                8,
                4 * widthPixels, 
                CGColorSpace.CreateDeviceRGB(),
                CGImageAlphaInfo.PremultipliedFirst))
            {
                RectangleF imageRect = new RectangleF(0, 0, widthPixels, heightPixels);

                // draw the image
                context.DrawImage(imageRect, originalImage.CGImage);

                UIImage resizedImage = UIImage.FromImage(context.ToImage(), 0, orientation);

                // save the image as a jpeg
                return Task.FromResult(resizedImage.AsJPEG().ToArray());
            }
        }

        public static UIImage ImageFromByteArray(byte[] data)
        {
            if (data == null)
            {
                return null;
            }

            UIImage image;
            try
            {
                image = new UIImage(Foundation.NSData.FromArray(data));
            }
            catch (Exception e)
            {
                Console.WriteLine("Image load failed: " + e.Message);
                return null;
            }

            return image;
        }

        public Task<byte[]> ResizeImageToHalfSizeAsync(byte[] imageData)
        {
            UIImage originalImage = ImageFromByteArray(imageData);

            int halfImageHeight = (int)originalImage.Size.Height / 2;
            int halfImageWidth = (int)originalImage.Size.Width / 2;

            UIImageOrientation orientation = originalImage.Orientation;

            // create a 24bit RGB image
            using (CGBitmapContext context = new CGBitmapContext(
                IntPtr.Zero,
                halfImageWidth, 
                halfImageHeight, 
                8,
                4 * halfImageWidth, 
                CGColorSpace.CreateDeviceRGB(),
                CGImageAlphaInfo.PremultipliedFirst))
            {
                RectangleF imageRect = new RectangleF(0, 0, halfImageWidth, halfImageHeight);

                // draw the image
                context.DrawImage(imageRect, originalImage.CGImage);

                UIImage resizedImage = UIImage.FromImage(context.ToImage(), 0, orientation);

                // save the image as a jpeg
                return Task.FromResult(resizedImage.AsJPEG().ToArray());
            }
        }
    }
}