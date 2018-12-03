using System;
using System.Collections.Generic;
using static Microsoft.Azure.Management.Fluent.Azure;

namespace VisualProvision.Services.Management.Deployment
{
    public interface IDeploymentManager
    {
        event EventHandler<DeploymentEventArgs> Started;

        event EventHandler<DeploymentEventArgs> Finished;

        event EventHandler<DeploymentErrorEventArgs> Failed;

        void Deploy(
            IAuthenticated azure,
            DeploymentOptions options, 
            IEnumerable<AzureResource> resources);
    }
}
