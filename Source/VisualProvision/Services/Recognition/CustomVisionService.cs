using Newtonsoft.Json;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace VisualProvision.Services.Recognition
{
    public class CustomVisionService
    {
        // <snippet_prediction>
        public async Task<PredictionResult> PredictImageContentsAsync(Stream imageStream, CancellationToken cancellationToken)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Prediction-key", AppSettings.CustomVisionPredictionKey);

            byte[] imageData = StreamToByteArray(imageStream);

            HttpResponseMessage response;
            using (var content = new ByteArrayContent(imageData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response = await client.PostAsync(AppSettings.CustomVisionPredictionUrl, content);
            }

            var resultJson = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<PredictionResult>(resultJson);
        }
        // </snippet_prediction>

        private byte[] StreamToByteArray(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }

                return ms.ToArray();
            }
        }
    }
}
