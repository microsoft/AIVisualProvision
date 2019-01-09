using System;
using System.Threading.Tasks;
using Microsoft.Azure.Management.Compute.Fluent.Models;
using static Microsoft.Azure.Management.Fluent.Azure;

namespace VisualProvision.Services.Management.Deployment
{
    public class VirtualMachineDeployment : BaseDeployment
    {
        public string VMName { get; private set; }

        public VirtualMachineDeployment(string vmName, IAuthenticated azure, DeploymentOptions options) : base(azure, options)
        {
            VMName = vmName;
        }

        protected override async Task ExecuteCreateAsync()
        {
                var definition = Azure
                        .WithSubscription(Options.SubscriptionId)
                        .VirtualMachines.Define(VMName)
                        .WithRegion(Options.Region);

                var withLogin = Options.UseExistingResourceGroup
                    ? definition.WithExistingResourceGroup(Options.ResourceGroupName)
                    : definition.WithNewResourceGroup(Options.ResourceGroupName);

                var virtualMachine = await withLogin
                    .WithNewPrimaryNetwork("10.0.0.0/28")
                    .WithPrimaryPrivateIPAddressDynamic()
                    .WithNewPrimaryPublicIPAddress(VMName)
                    .WithLatestWindowsImage("MicrosoftWindowsServer", "WindowsServer", "2016-Datacenter")
                    .WithAdminUsername("windowsadmin")
                    .WithAdminPassword("P@assw0rd")
                    .WithSize(VirtualMachineSizeTypes.StandardA1)
                    .CreateAsync();
        }

        protected override string GetDeploymentName()
        {
            return $"'{VMName}' Virtual Machine";
        }

        protected override string GetEventName()
        {
            return "Virtual Machine";
        }
    }
}