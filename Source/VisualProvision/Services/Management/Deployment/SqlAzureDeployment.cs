using Microsoft.Azure.Management.Sql.Fluent.Models;
using Microsoft.Azure.Management.Sql.Fluent.SqlServer.Definition;
using System.Threading.Tasks;
using static Microsoft.Azure.Management.Fluent.Azure;

namespace VisualProvision.Services.Management.Deployment
{
    public class SqlAzureDeployment : BaseDeployment
    {
        public SqlAzureDeployment(
            string serverName,
            IAuthenticated azure,
            DeploymentOptions options)
            : base(azure, options)
        {
            ServerName = serverName;
        }

        public string ServerName { get; private set; }

        protected override async Task ExecuteCreateAsync()
        {
            var definition = Azure
                .WithSubscription(Options.SubscriptionId)
                .SqlServers.Define(ServerName)
                .WithRegion(Options.Region);

            IWithAdministratorLogin withLogin = Options.UseExistingResourceGroup
                ? definition.WithExistingResourceGroup(Options.ResourceGroupName)
                : definition.WithNewResourceGroup(Options.ResourceGroupName);

            var sqlServer = await withLogin
                .WithAdministratorLogin("demo")
                .WithAdministratorPassword("P@assw0rd")
                .CreateAsync();

            var database = await sqlServer.Databases.Define($"{ServerName}_db")
                .WithEdition(DatabaseEditions.Basic)
                .CreateAsync();
        }

        protected override string GetDeploymentName()
        {
            return $"'{ServerName}' SQL Azure";
        }

        protected override string GetEventName()
        {
            return "SQL Azure";
        }
    }
}
