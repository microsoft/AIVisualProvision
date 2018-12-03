using System;
using System.Collections.Generic;
using System.Linq;

namespace VisualProvision.Services.Management
{
    public class AzureResourceManager
    {
        private static readonly Lazy<AzureResourceManager> lazy = new Lazy<AzureResourceManager>(() => new AzureResourceManager());

        private AzureResourceManager()
        {
            InitializeResources();
        }

        public static AzureResourceManager Instance => lazy.Value;

        public List<AzureResource> AvailableResources { get; private set; }

        public List<AzureResource> ResourcesToDeploy { get; set; } = new List<AzureResource>();

        private void InitializeResources()
        {
            var allResources = new List<AzureResource>();

            allResources.Add(new AzureResource()
            {
                Type = AzureResourceType.ActiveDirectory,
                Name = AzureResourceNamingHelper.AZURE_ACTIVE_DIRECTORY,
                Description = AzureResourceNamingHelper.AZURE_ACTIVE_DIRECTORY,
                IsAvailable = false,
            });

            allResources.Add(new AzureResource()
            {
                Type = AzureResourceType.AppService,
                Name = AzureResourceNamingHelper.APP_SERVICE,
                Description = AzureResourceNamingHelper.APP_SERVICE,
                IsAvailable = true,
            });

            allResources.Add(new AzureResource()
            {
                Type = AzureResourceType.NotificationHubs,
                Name = AzureResourceNamingHelper.NOTIFICATION_HUBS,
                Description = AzureResourceNamingHelper.NOTIFICATION_HUBS,
                IsAvailable = false,
            });

            allResources.Add(new AzureResource()
            {
                Type = AzureResourceType.MobileApps,
                Name = AzureResourceNamingHelper.MOBILE_APPS,
                Description = AzureResourceNamingHelper.MOBILE_APPS,
                IsAvailable = false,
            });

            allResources.Add(new AzureResource()
            {
                Type = AzureResourceType.AzureSearch,
                Name = AzureResourceNamingHelper.AZURE_SEARCH,
                Description = AzureResourceNamingHelper.AZURE_SEARCH,
                IsAvailable = false,
            });

            allResources.Add(new AzureResource()
            {
                Type = AzureResourceType.Cdn,
                Name = AzureResourceNamingHelper.AZURE_CDN,
                Description = AzureResourceNamingHelper.AZURE_CDN,
                IsAvailable = false,
            });

            allResources.Add(new AzureResource()
            {
                Type = AzureResourceType.MachineLearning,
                Name = AzureResourceNamingHelper.AZURE_MACHINE_LEARNING,
                Description = AzureResourceNamingHelper.AZURE_MACHINE_LEARNING,
                IsAvailable = false,
            });

            allResources.Add(new AzureResource()
            {
                Type = AzureResourceType.Storage,
                Name = AzureResourceNamingHelper.AZURE_STORAGE,
                Description = AzureResourceNamingHelper.AZURE_STORAGE,
                IsAvailable = true,
            });

            allResources.Add(new AzureResource()
            {
                Type = AzureResourceType.IotEdge,
                Name = AzureResourceNamingHelper.IOT_EDGE,
                Description = AzureResourceNamingHelper.IOT_EDGE,
                IsAvailable = false,
            });

            allResources.Add(new AzureResource()
            {
                Type = AzureResourceType.CosmosDB,
                Name = AzureResourceNamingHelper.COSMOS_DB,
                Description = AzureResourceNamingHelper.COSMOS_DB,
                IsAvailable = true,
            });

            allResources.Add(new AzureResource()
            {
                Type = AzureResourceType.CognitiveServices,
                Name = AzureResourceNamingHelper.COGNITIVE_SERVICES,
                Description = AzureResourceNamingHelper.COGNITIVE_SERVICES,
                IsAvailable = false,
            });

            allResources.Add(new AzureResource()
            {
                Type = AzureResourceType.SqlDatabase,
                Name = AzureResourceNamingHelper.SQL_DATABASE,
                Description = AzureResourceNamingHelper.SQL_DATABASE,
                IsAvailable = true,
            });

            allResources.Add(new AzureResource()
            {
                Type = AzureResourceType.MySqlClearDatabase,
                Name = AzureResourceNamingHelper.AZURE_MYSQL_CLEARDB_DATABASE,
                Description = AzureResourceNamingHelper.AZURE_MYSQL_CLEARDB_DATABASE,
                IsAvailable = false,
            });

            allResources.Add(new AzureResource()
            {
                Type = AzureResourceType.RedisCache,
                Name = AzureResourceNamingHelper.REDIS_CACHE,
                Description = AzureResourceNamingHelper.REDIS_CACHE,
                IsAvailable = false,
            });

            allResources.Add(new AzureResource()
            {
                Type = AzureResourceType.ApplicationInsights,
                Name = AzureResourceNamingHelper.APP_INSIGHTS,
                Description = AzureResourceNamingHelper.APP_INSIGHTS,
                IsAvailable = false,
            });

            allResources.Add(new AzureResource()
            {
                Type = AzureResourceType.Functions,
                Name = AzureResourceNamingHelper.AZURE_FUNCTIONS,
                Description = AzureResourceNamingHelper.AZURE_FUNCTIONS,
                IsAvailable = true,
            });

            allResources.Add(new AzureResource()
            {
                Type = AzureResourceType.WebApp,
                Name = AzureResourceNamingHelper.WEB_APPS,
                Description = AzureResourceNamingHelper.WEB_APPS,
                IsAvailable = true,
            });

            allResources.Add(new AzureResource()
            {
                Type = AzureResourceType.KeyVault,
                Name = AzureResourceNamingHelper.KEY_VAULT,
                Description = AzureResourceNamingHelper.KEY_VAULT,
                IsAvailable = true,
            });

            AvailableResources = allResources.Where(r => r.IsAvailable).ToList();
        }
    }
}
