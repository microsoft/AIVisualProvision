using Microsoft.Azure.Management.AppService.Fluent.FunctionApp.Definition;
using System.Threading.Tasks;
using static Microsoft.Azure.Management.Fluent.Azure;

namespace VisualProvision.Services.Management.Deployment
{
    public class AzureFunctionDeployment : BaseDeployment
    {
        public AzureFunctionDeployment(
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
            INewAppServicePlanWithGroup definition = Azure
                .WithSubscription(Options.SubscriptionId)
                .AppServices.FunctionApps
                .Define(AppName)
                .WithRegion(Options.Region);

            IWithCreate create = Options.UseExistingResourceGroup
                ? definition.WithExistingResourceGroup(Options.ResourceGroupName)
                : definition.WithNewResourceGroup(Options.ResourceGroupName);

            return create.CreateAsync();
        }

        protected override string GetDeploymentName()
        {
            return $"'{AppName}' Azure Function";
        }

        protected override string GetEventName()
        {
            return "Azure Functions";
        }
    }
}
