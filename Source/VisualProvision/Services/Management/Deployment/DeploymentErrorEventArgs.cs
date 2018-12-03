using VisualProvision.Models;
using System;

namespace VisualProvision.Services.Management.Deployment
{
    public class DeploymentErrorEventArgs : DeploymentEventArgs
    {
        public DeploymentErrorEventArgs(AzureResource resource, Exception exception)
            : base(resource)
        {
            Exception = exception;
        }

        public Exception Exception { get; private set; }
    }
}
