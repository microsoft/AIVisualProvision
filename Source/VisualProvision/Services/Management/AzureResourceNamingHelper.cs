using System.Text.RegularExpressions;

namespace VisualProvision.Services.Management
{
    public class AzureResourceNamingHelper
    {
        // Full names
        public const string AZURE_ACTIVE_DIRECTORY = "Azure Active Directory";
        public const string APP_SERVICE = "App Service";
        public const string NOTIFICATION_HUBS = "Notification Hubs";
        public const string MOBILE_APPS = "Mobile Apps";
        public const string AZURE_SEARCH = "Azure Search";
        public const string AZURE_CDN = "Azure CDN";
        public const string AZURE_MACHINE_LEARNING = "Azure Machine Learning";
        public const string AZURE_STORAGE = "Azure Storage";
        public const string IOT_EDGE = "IoT Edge";
        public const string COSMOS_DB = "Cosmos DB";
        public const string COGNITIVE_SERVICES = "Cognitive Services";
        public const string SQL_DATABASE = "SQL Database";
        public const string AZURE_MYSQL_CLEARDB_DATABASE = "Azure MySQL ClearDB Database";
        public const string REDIS_CACHE = "Redis Cache";
        public const string APP_INSIGHTS = "Application Insights";
        public const string AZURE_FUNCTIONS = "Azure Functions";
        public const string WEB_APPS = "Web Apps";
        public const string KEY_VAULT = "Key Vault";
        public const string VIRTUAL_MACHINE = "Virtual Machine";

        private static Regex ResourceGroupNameRegex = new Regex(@"^[-\w\._\(\)]+$", RegexOptions.Compiled);

        public static bool CheckResourceGroupName(string candidate)
        {
            return ResourceGroupNameRegex.IsMatch(candidate ?? string.Empty);
        }
    }
}
