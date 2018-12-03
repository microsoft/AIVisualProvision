using Microsoft.Azure.Management.CosmosDB.Fluent.CosmosDBAccount.Definition;
using Microsoft.Azure.Management.CosmosDB.Fluent.Models;
using System.Threading.Tasks;
using static Microsoft.Azure.Management.Fluent.Azure;

namespace VisualProvision.Services.Management.Deployment
{
    public class CosmosDbAccountDeployment : BaseDeployment
    {
        public CosmosDbAccountDeployment(
            string docDbName,
            IAuthenticated azure,
            DeploymentOptions options) 
            : base(azure, options)
        {
            DocDbName = docDbName;
        }

        public string DocDbName { get; private set; }

        protected override Task ExecuteCreateAsync()
        {
            var definition = Azure
                .WithSubscription(Options.SubscriptionId)
                .CosmosDBAccounts.Define(DocDbName)
                .WithRegion(Options.Region);

            IWithKind kind = Options.UseExistingResourceGroup
                ? definition.WithExistingResourceGroup(Options.ResourceGroupName)
                : definition.WithNewResourceGroup(Options.ResourceGroupName);

            return kind
                .WithKind(DatabaseAccountKind.GlobalDocumentDB)
                .WithSessionConsistency()
                .WithWriteReplication(Options.Region)
                .WithReadReplication(Options.Region)
                .CreateAsync();
        }

        protected override string GetDeploymentName()
        {
            return $"'{DocDbName}' CosmosDB Account";
        }

        protected override string GetEventName()
        {
            return "Azure CosmosDB";
        }
    }
}
