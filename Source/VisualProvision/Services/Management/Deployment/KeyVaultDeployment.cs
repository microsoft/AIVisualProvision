using Microsoft.Azure.Management.KeyVault.Fluent.Vault.Definition;
using System.Threading.Tasks;
using static Microsoft.Azure.Management.Fluent.Azure;

namespace VisualProvision.Services.Management.Deployment
{
    public class KeyVaultDeployment : BaseDeployment
    {
        public KeyVaultDeployment(
            string vaultName, 
            IAuthenticated azure,
            DeploymentOptions options)
            : base(azure, options)
        {
            VaultName = vaultName;
        }

        public string VaultName { get; private set; }

        protected override Task ExecuteCreateAsync()
        {
            var definition = Azure
                .WithSubscription(Options.SubscriptionId)
                .Vaults.Define(VaultName)
                .WithRegion(Options.Region);

            IWithAccessPolicy withPolicy = Options.UseExistingResourceGroup
                ? definition.WithExistingResourceGroup(Options.ResourceGroupName)
                : definition.WithNewResourceGroup(Options.ResourceGroupName);

            return withPolicy
                .WithEmptyAccessPolicy()
                .CreateAsync();
        }

        protected override string GetDeploymentName()
        {
            return $"'{VaultName}' Key Vault";
        }

        protected override string GetEventName()
        {
            return "Key Vault";
        }
    }
}
