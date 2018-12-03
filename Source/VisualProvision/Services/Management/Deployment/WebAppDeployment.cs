using Microsoft.Azure.Management.AppService.Fluent;
using Microsoft.Azure.Management.AppService.Fluent.WebApp.Definition;
using System.Threading.Tasks;
using static Microsoft.Azure.Management.Fluent.Azure;

namespace VisualProvision.Services.Management.Deployment
{
    public class WebAppDeployment : BaseDeployment
    {
        public WebAppDeployment(
            string appName,
            IAuthenticated azure,
            DeploymentOptions options)
            : base(azure, options)
        {
            AppName = appName;
        }

        public string AppName { get; private set; }

        protected override Task ExecuteCreateAsync()
        {
            var definition = Azure
              .WithSubscription(Options.SubscriptionId)
              .WebApps.Define(AppName)
              .WithRegion(Options.Region);

            IWithNewAppServicePlan create = Options.UseExistingResourceGroup
                ? definition.WithExistingResourceGroup(Options.ResourceGroupName)
                : definition.WithNewResourceGroup(Options.ResourceGroupName);

            return create
                .WithNewWindowsPlan(PricingTier.FreeF1)
                .CreateAsync();
        }

        protected override string GetDeploymentName()
        {
            return $"'{AppName}' Web App";
        }

        protected override string GetEventName()
        {
            return "Azure Web Apps";
        }
    }
}
