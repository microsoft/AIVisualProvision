using VisualProvision.Services.Management;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace VisualProvision.Converters
{
    public class ServiceIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var type = value as AzureResourceType?;

            if (type == null)
            {
                return null;
            }

            switch (type)
            {
                case AzureResourceType.AppService:
                case AzureResourceType.WebApp:
                    return "ic_app_service.png";
                case AzureResourceType.Functions:
                    return "ic_azure_functions.png";
                case AzureResourceType.MobileApps:
                    return "ic_mobile_apps.png";
                case AzureResourceType.ApplicationInsights:
                    return "ic_application_insights.png";
                case AzureResourceType.ActiveDirectory:
                    return "ic_azure_active_directory.png";
                case AzureResourceType.MachineLearning:
                    return "ic_azure_machine_learning.png";
                case AzureResourceType.Storage:
                    return "ic_azure_storage.png";
                case AzureResourceType.IotEdge:
                    return "ic_io_t_edge.png";
                case AzureResourceType.CosmosDB:
                    return "ic_cosmos_db.png";
                case AzureResourceType.CognitiveServices:
                    return "ic_cognitive_service.png";
                case AzureResourceType.SqlDatabase:
                    return "ic_sql_database.png";
                case AzureResourceType.KeyVault:
                    return "ic_key_vault.png";
                case AzureResourceType.VirtualMachine:
                    return "ic_virtual_machine.png";
                default:
                    return string.Empty;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}