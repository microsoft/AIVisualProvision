using System.Threading.Tasks;
using Microsoft.Azure.Management.Storage.Fluent.StorageAccount.Definition;
using static Microsoft.Azure.Management.Fluent.Azure;

namespace VisualProvision.Services.Management.Deployment
{
    public class StorageAccountDeployment : BaseDeployment
    {
        public StorageAccountDeployment(
            string accountName,
            IAuthenticated azure,
            DeploymentOptions options)
            : base(azure, options)
        {
            AccountName = accountName;
        }

        public string AccountName { get; private set; }

        protected override Task ExecuteCreateAsync()
        {
            var definition = Azure
               .WithSubscription(Options.SubscriptionId)
               .StorageAccounts.Define(AccountName)
               .WithRegion(Options.Region);

            IWithCreate create = Options.UseExistingResourceGroup
                ? definition.WithExistingResourceGroup(Options.ResourceGroupName)
                : definition.WithNewResourceGroup(Options.ResourceGroupName);

            return create.WithBlobStorageAccountKind()
                .CreateAsync();
        }

        protected override string GetDeploymentName()
        {
            return $"'{AccountName}' CosmosDB Account";
        }

        protected override string GetEventName()
        {
            return "Azure Storage";
        }
    }
}
