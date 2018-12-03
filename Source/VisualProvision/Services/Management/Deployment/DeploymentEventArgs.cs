using VisualProvision.Models;
using System;

namespace VisualProvision.Services.Management.Deployment
{
    public class DeploymentEventArgs : EventArgs
    {
        public DeploymentEventArgs(AzureResource resource)
        {
            Resource = resource;
        }

        public AzureResource Resource { get; private set; }
    }
}
