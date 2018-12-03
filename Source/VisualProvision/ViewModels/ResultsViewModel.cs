using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using VisualProvision.Models;
using VisualProvision.Services.Management;
using VisualProvision.Services.Management.Deployment;
using Xamarin.Forms;

namespace VisualProvision.ViewModels
{
    public class ResultsViewModel : BaseViewModel
    {
        private readonly IDeploymentManager manager;
        private bool deploymentDone;
        private int remainingDeployments;
        private List<AzureResourceModel> azureResources;

        public ResultsViewModel()
        {
            manager = DependencyService.Get<IDeploymentManager>();
        }

        public List<AzureResourceModel> AzureResources
        {
            get => azureResources;

            set => SetProperty(ref azureResources, value);
        }

        public ICommand DoneCommand => new Command(async (o) => await DoneAsync());

        public bool DeploymentDone
        {
            get => deploymentDone;

            set => SetProperty(ref deploymentDone, value);
        }

        public override async Task NavigatedToAsync(object navigationParameter)
        {
            await base.NavigatedToAsync(navigationParameter);

            SubscribeManager();

            CreateResources();
        }

        public override Task NavigatedFromAsync()
        {
            UnsubscribeManager();
            return base.NavigatedFromAsync();
        }

        private void CreateResources()
        {
            List<AzureResource> detectedResources = AzureResourceManager.Instance.ResourcesToDeploy;

            AzureResources = detectedResources?
                .Select(AzureResourceModel.FromResource)
                .ToList();

            DeploymentDone = false;
            remainingDeployments = AzureResources.Count();

            manager.Deploy(
                AzureService.Authenticated,
                AzureService.DeploymentOptions,
                detectedResources);
        }

        private Task DoneAsync()
        {
            return NavigationService.ClearNavigateToAsync<SubscriptionViewModel>();
        }

        private void SubscribeManager()
        {
            manager.Started += OnDeploymentStarted;
            manager.Finished += OnDeploymentFinished;
            manager.Failed += OnDeploymentFailed;
        }

        private void UnsubscribeManager()
        {
            manager.Started -= OnDeploymentStarted;
            manager.Finished -= OnDeploymentFinished;
            manager.Failed -= OnDeploymentFailed;
        }

        private void OnDeploymentStarted(object sender, DeploymentEventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                AzureResourceModel model = GetModelFromResource(e.Resource);

                if (model != null)
                {
                    model.IsDeploying = true;
                }
            });
        }

        private void OnDeploymentFailed(object sender, DeploymentErrorEventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                AzureResourceModel model = GetModelFromResource(e.Resource);

                if (model != null)
                {
                    model.IsDeploying = false;
                    model.IsCreated = false;
                    model.HasFailed = true;
                }

                remainingDeployments--;
                DeploymentDone = remainingDeployments <= 0;
            });
        }

        private void OnDeploymentFinished(object sender, DeploymentEventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                AzureResourceModel model = GetModelFromResource(e.Resource);

                if (model != null)
                {
                    model.IsDeploying = false;
                    model.IsCreated = true;
                }

                remainingDeployments--;
                DeploymentDone = remainingDeployments <= 0;
            });
        }

        private AzureResourceModel GetModelFromResource(AzureResource resource)
        {
            return AzureResources?.FirstOrDefault(r => r.Type == resource.Type);
        }
    }
}