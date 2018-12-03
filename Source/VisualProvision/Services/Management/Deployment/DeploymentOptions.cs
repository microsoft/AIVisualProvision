using Microsoft.Azure.Management.ResourceManager.Fluent.Core;

namespace VisualProvision.Services.Management.Deployment
{
    public class DeploymentOptions
    {
        public string SubscriptionId { get; set; }

        public string ResourceGroupName { get; set; }

        public bool UseExistingResourceGroup { get; set; }

        public Region Region { get; set; }

        public static DeploymentOptions Default
        {
            get => new DeploymentOptions
            {
                Region = Region.USWest,
                ResourceGroupName = nameof(VisualProvision),
                UseExistingResourceGroup = false,
            };
        }
    }
}
