using System.Collections.Generic;
using System.Threading.Tasks;
using Plugin.Media.Abstractions;
using VisualProvision.Services.Management;

namespace VisualProvision.Services.Recognition
{
    public interface IRecognitionService
    {
        Task<IEnumerable<AzureResource>> GetResourcesAsync(MediaFile image);
    }
}
