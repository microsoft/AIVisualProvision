using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Plugin.Media.Abstractions;
using VisualProvision.Services.Management;
using Xamarin.Forms;

namespace VisualProvision.Services.Recognition
{
    public class RecognitionService : IRecognitionService
    {
        private const int MaxUploadImageSizeMB = 4;
        private const int MaxUploadImageSizeBytes = MaxUploadImageSizeMB * 1024 * 1000;
        private const double PrecisionThreshold = 0.5;

        private const string TAG_ACTIVE_DIRECTORY = "ACTIVE_DIRECTORY";
        private const string TAG_APP_SERVICE = "APP_SERVICE";
        private const string TAG_NOTIFICATION_HUBS = "NOTIFICATION_HUBS";
        private const string TAG_MOBILE_APPS = "MOBILE_APPS";
        private const string TAG_AZURE_SEARCH = "AZURE_SEARCH";
        private const string TAG_AZURE_CDN = "CDN";
        private const string TAG_AZURE_MACHINE_LEARNING = "MACHINE_LEARNING";
        private const string TAG_AZURE_STORAGE = "STORAGE";
        private const string TAG_IOT_EDGE = "IOT_EDGE";
        private const string TAG_COSMOS_DB = "COSMOS_DB";
        private const string TAG_COGNITIVE_SERVICES = "COGNITIVE_SERVICES";
        private const string TAG_SQL_DATABASE = "SQL_DATABASE";
        private const string TAG_MYSQL_CLEARDB_DATABASE = "MYSQL_CLEARDB_DATABASE";
        private const string TAG_REDIS_CACHE = "REDIS_CACHE";
        private const string TAG_APP_INSIGHTS = "APPLICATION_INSIGHTS";
        private const string TAG_AZURE_FUNCTIONS = "AZURE_FUNCTIONS";

        public async Task<IEnumerable<AzureResource>> GetResourcesAsync(MediaFile image)
        {
            Stream properStream = await GetCapturedPhotoStreamAsync(image);
            Stream anotherProperStream = await GetCapturedPhotoStreamAsync(image);

            var textRecognitionService = new ComputerVisionService();

            Task<PredictionResult> logoPredictionTask = GetRawCustomVisionPrediction(properStream);
            Task<TextOperationResult> textPredictionTask = textRecognitionService.RecognizeTextAsync(
                anotherProperStream,
                default(System.Threading.CancellationToken));

            await Task.WhenAll(logoPredictionTask, textPredictionTask);

            PredictionResult logoPredictionResult = logoPredictionTask.Result;
            TextOperationResult textPredictionResult = textPredictionTask.Result;

            // Logos part
            List<string> filteredLogoPredictionResult = logoPredictionResult
                .Predictions
                .Where(prediction => prediction.Probability > PrecisionThreshold)
                .Select(prediction => prediction.TagName)
                .ToList();

            List<AzureResource> servicesDetectedByLogoList = 
                ConvertTagNamesToAzureResources(filteredLogoPredictionResult);

            // Handwriting part
            List<AzureResource> servicesDetectedByHandwritingList = 
                ConvertTextResultToAzureResources(textPredictionResult);

            IEnumerable<AzureResource> results = servicesDetectedByLogoList
                .Union(servicesDetectedByHandwritingList);

            return results;
        }

        private async Task<Stream> GetCapturedPhotoStreamAsync(MediaFile capturedImage)
        {
            var stream = capturedImage.GetStream();
            if (stream.Length > MaxUploadImageSizeBytes)
            {
                using (var memstream = new MemoryStream())
                {
                    stream.CopyTo(memstream);
                    var imageByteArray = memstream.ToArray();

                    var imageResizer = DependencyService.Get<IImageResizerService>();
                    byte[] resizedImage = await imageResizer.ResizeImageToHalfSizeAsync(imageByteArray);
                    stream = new MemoryStream(resizedImage);
                }
            }

            return stream;
        }

        private async Task<PredictionResult> GetRawCustomVisionPrediction(Stream stream)
        {
            var cancellationToken = default(System.Threading.CancellationToken);
            var visionService = new CustomVisionService();
            return await visionService.PredictImageContentsAsync(stream, cancellationToken);
        }

        private static AzureResourceType? TagNameToType(string tagName)
        {
            switch (tagName.ToUpper())
            {
                case TAG_ACTIVE_DIRECTORY:
                    return AzureResourceType.ActiveDirectory;
                case TAG_APP_SERVICE:
                    return AzureResourceType.AppService;
                case TAG_NOTIFICATION_HUBS:
                    return AzureResourceType.NotificationHubs;
                case TAG_MOBILE_APPS:
                    return AzureResourceType.MobileApps;
                case TAG_AZURE_SEARCH:
                    return AzureResourceType.AzureSearch;
                case TAG_AZURE_CDN:
                    return AzureResourceType.Cdn;
                case TAG_AZURE_MACHINE_LEARNING:
                    return AzureResourceType.MachineLearning;
                case TAG_AZURE_STORAGE:
                    return AzureResourceType.Storage;
                case TAG_IOT_EDGE:
                    return AzureResourceType.IotEdge;
                case TAG_COSMOS_DB:
                    return AzureResourceType.CosmosDB;
                case TAG_COGNITIVE_SERVICES:
                    return AzureResourceType.CognitiveServices;
                case TAG_SQL_DATABASE:
                    return AzureResourceType.SqlDatabase;
                case TAG_MYSQL_CLEARDB_DATABASE:
                    return AzureResourceType.MySqlClearDatabase;
                case TAG_REDIS_CACHE:
                    return AzureResourceType.RedisCache;
                case TAG_APP_INSIGHTS:
                    return AzureResourceType.ApplicationInsights;
                case TAG_AZURE_FUNCTIONS:
                    return AzureResourceType.Functions;
                default:
                    return null;
            }
        }

        private static List<AzureResource> ConvertTagNamesToAzureResources(List<string> tagNames)
        {
            List<AzureResource> allResources = AzureResourceManager.Instance.AvailableResources;

            return allResources.Where(resource => tagNames.Any(t => TagNameToType(t) == resource.Type))
                .ToList();
        }

        private static List<AzureResource> ConvertTextResultToAzureResources(TextOperationResult predictionResult)
        {
            var types = new List<AzureResourceType>();

            foreach (var line in predictionResult.RecognitionResult.Lines)
            {
                var wordsList = line.Words.Select(word => word.Text).ToList();

                // Case insensitive
                wordsList = wordsList.ConvertAll(w => w.ToLower());

                // Remove punctiation marks
                wordsList = wordsList.ConvertAll(w => w.Replace("[.,]+", string.Empty));

                foreach (var pattern in RecognizedTextToAzureTag.Keys)
                {
                    // Check that contains all words in pattern
                    if (pattern.Intersect(wordsList).Count() == pattern.Count())
                    {
                        types.Add(RecognizedTextToAzureTag[pattern]);
                    }
                }
            }

            List<AzureResource> allResources = AzureResourceManager.Instance.AvailableResources;
            return allResources.Where(r => types.Contains(r.Type)).ToList();
        }

        private static readonly Dictionary<List<string>, AzureResourceType> RecognizedTextToAzureTag = new Dictionary<List<string>, AzureResourceType>()
        {
            { new List<string>() { "active", "directory" }, AzureResourceType.ActiveDirectory },
            { new List<string>() { "app", "service" }, AzureResourceType.AppService },
            { new List<string>() { "application", "service" }, AzureResourceType.AppService },
            { new List<string>() { "web", "app" }, AzureResourceType.AppService },
            { new List<string>() { "web", "apps" }, AzureResourceType.AppService },
            { new List<string>() { "notification", "hubs" }, AzureResourceType.NotificationHubs },
            { new List<string>() { "mobile", "apps" }, AzureResourceType.MobileApps },
            { new List<string>() { "search" }, AzureResourceType.AzureSearch },
            { new List<string>() { "cdn" }, AzureResourceType.Cdn },
            { new List<string>() { "machine", "learning" }, AzureResourceType.MachineLearning },
            { new List<string>() { "storage" }, AzureResourceType.Storage },
            { new List<string>() { "iot", "edge" }, AzureResourceType.IotEdge },
            { new List<string>() { "cosmos", "db" }, AzureResourceType.CosmosDB },
            { new List<string>() { "cosmosdb" }, AzureResourceType.CosmosDB },
            { new List<string>() { "cognitive", "services" }, AzureResourceType.CognitiveServices },
            { new List<string>() { "sql", "database" }, AzureResourceType.SqlDatabase },
            { new List<string>() { "mysql", "database" }, AzureResourceType.SqlDatabase },
            { new List<string>() { "sql", "db" }, AzureResourceType.SqlDatabase },
            { new List<string>() { "mysql", "db" }, AzureResourceType.SqlDatabase },
            { new List<string>() { "sql", "server" }, AzureResourceType.SqlDatabase },
            { new List<string>() { "mysql", "server" }, AzureResourceType.SqlDatabase },
            { new List<string>() { "mysql", "cleardb" }, AzureResourceType.MySqlClearDatabase },
            { new List<string>() { "mysql", "cleardb", "database" }, AzureResourceType.MySqlClearDatabase },
            { new List<string>() { "cleardb", "database" }, AzureResourceType.MySqlClearDatabase },
            { new List<string>() { "redis", "cache" }, AzureResourceType.RedisCache },
            { new List<string>() { "app", "insights" }, AzureResourceType.ApplicationInsights },
            { new List<string>() { "application", "insights" }, AzureResourceType.ApplicationInsights },
            { new List<string>() { "functions" }, AzureResourceType.Functions },
            { new List<string>() { "function" }, AzureResourceType.Functions },
            { new List<string>() { "key", "vault" }, AzureResourceType.KeyVault },
            { new List<string>() { "keyvault" }, AzureResourceType.KeyVault },
        };
    }
}
