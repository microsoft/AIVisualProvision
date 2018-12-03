using System.Threading.Tasks;

namespace VisualProvision.Services
{
    public interface IImageResizerService
    {
        Task<byte[]> ResizeImageAsync(byte[] imageData, int maxWidthPixels, int maxHeightPixels);

        Task<byte[]> ResizeImageToHalfSizeAsync(byte[] imageData);
    }
}
