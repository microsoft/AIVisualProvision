using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace VisualProvision.Services.Recognition
{
    public class ComputerVisionService
    {
        private const int NumberOfCharsInOperationId = 36;
        private const TextRecognitionMode Mode = TextRecognitionMode.Handwritten;

        public async Task<TextOperationResult> RecognizeTextAsync(Stream imageStream, CancellationToken cancellationToken)
        {
            var credentials = new ApiKeyServiceClientCredentials(AppSettings.ComputerVisionKey);

            using (var client = new ComputerVisionClient(credentials) { Endpoint = AppSettings.ComputerVisionEndpoint })
            {
                RecognizeTextInStreamHeaders textHeaders =
                    await client.RecognizeTextInStreamAsync(imageStream, Mode, cancellationToken);

                // Go for the result
                var result = await GetTextAsync(client, textHeaders.OperationLocation, cancellationToken);

                return result;
            }
        }

        private async Task<TextOperationResult> GetTextAsync(
            ComputerVisionClient computerVision,
            string operationLocation,
            CancellationToken cancellationToken)
        {
            // Retrieve the URI where the recognized text will be stored from the Operation-Location header
            string operationId = operationLocation.Substring(operationLocation.Length - NumberOfCharsInOperationId);

            TextOperationResult result = await computerVision.GetTextOperationResultAsync(operationId, cancellationToken);

            // Wait for the operation to complete
            int i = 0;
            int maxRetries = 10;
            while ((result.Status == TextOperationStatusCodes.Running ||
                    result.Status == TextOperationStatusCodes.NotStarted) && i++ < maxRetries)
            {
                System.Diagnostics.Debug.WriteLine(
                    "Server status: {0}, waiting {1} seconds...", result.Status, i);
                await Task.Delay(1000);

                result = await computerVision.GetTextOperationResultAsync(operationId, cancellationToken);
            }

            return result;
        }
    }
}
