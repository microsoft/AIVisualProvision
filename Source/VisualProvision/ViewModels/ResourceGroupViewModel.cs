using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using VisualProvision.Models;
using VisualProvision.Resources;
using VisualProvision.Services.Management;
using VisualProvision.Services.Management.Deployment;
using VisualProvision.Utils.Validations;
using Xamarin.Forms;

namespace VisualProvision.ViewModels
{
    public class ResourceGroupViewModel : BaseViewModel
    {
        private readonly SubscriptionsCache subscriptionCache;
        private List<Ubication> regions;
        private Ubication region;
        private ObservableCollection<ResourceGroup> resourceGroups;
        private ResourceGroup resourceGroup;

        private bool useExistingResourceGroup;

        private ValidatableObject<string> newResourceGroupName;
        private bool canContinue;

        public ResourceGroupViewModel()
        {
            useExistingResourceGroup = true;
            newResourceGroupName = new ValidatableObject<string>();
            newResourceGroupName.Validations.Add(
                new IsNotNullOrEmptyRule()
                {
                    ValidationMessage = Translations.Validations_ResourceGroup_RequiredName,
                });
            newResourceGroupName.Validations.Add(
                new EvalValidationRule<string>(
                    CheckResourceGroupName,
                    Translations.Validations_ResourceGroup_InvalidName));
            newResourceGroupName.Validations.Add(
                new EvalValidationRule<string>(
                    CheckResourceGroupUnique,
                    Translations.Validations_ResourceGroup_DuplicatedName));

            subscriptionCache = DependencyService.Get<SubscriptionsCache>();
        }

        public List<Ubication> Regions
        {
            get => regions;

            set => SetProperty(ref regions, value);
        }

        public Ubication Region
        {
            get => region;

            set 
            {
                if (SetProperty(ref region, value))
                {
                    CalculateCanContinue();
                }
            }
        }

        public ObservableCollection<ResourceGroup> ResourceGroups
        {
            get => resourceGroups;

            set => SetProperty(ref resourceGroups, value);
        }

        public ResourceGroup ResourceGroup
        {
            get => resourceGroup;

            set
            {
                if (resourceGroup != null)
                {
                    resourceGroup.Selected = false;
                }

                resourceGroup = value;

                if (resourceGroup != null)
                {
                    resourceGroup.Selected = true;
                }

                CalculateCanContinue();
            }
        }

        public bool UseExistingResourceGroup
        {
            get => useExistingResourceGroup;

            set
            {
                if (SetProperty(ref useExistingResourceGroup, value))
                {
                    UseExistingResourceGroupChanged();
                }
            }
        }

        public ValidatableObject<string> NewResourceGroupName
        {
            get => newResourceGroupName;

            set => SetProperty(ref newResourceGroupName, value);
        }

        public bool CanContinue
        {
            get => canContinue;

            set => SetProperty(ref canContinue, value);
        }

        public override async Task NavigatedToAsync(object navigationParameter)
        {
            IsBusy = true;

            await base.NavigatedToAsync(navigationParameter);

            List<Ubication> availableRegions = new List<Ubication>();
            List<ResourceGroup> allResourceGroups = new List<ResourceGroup>();

            await Task.Factory.StartNew(LoadDataAsync)
            .ContinueWith(
                task => 
                {
                    CalculateCanContinue();
                    IsBusy = false;
                }, 
                TaskScheduler.FromCurrentSynchronizationContext());
        }

        public ICommand NavigateToDeployCommand => new Command(async (o) => await NavigateToDeployAsync());

        private static bool CheckResourceGroupName(string groupName)
        {
            return AzureResourceNamingHelper.CheckResourceGroupName(groupName);
        }

        private async Task LoadDataAsync()
        {
            IEnumerable<Ubication> availableRegions = await AzureService.GetRegionsAsync();
            IEnumerable<ResourceGroup> allResourceGroups = await AzureService.GetResourceGroupsAsync();

            Regions = new List<Ubication>(availableRegions);
            ResourceGroups = new ObservableCollection<ResourceGroup>(allResourceGroups);
        }

        private async Task NavigateToDeployAsync()
        {
            if (CanContinue)
            {
                string resourceName = UseExistingResourceGroup
                    ? ResourceGroup?.Name
                    : NewResourceGroupName.Value;

                string subscriptionId = await subscriptionCache.GetCurrentAsync();

                AzureService.DeploymentOptions = new DeploymentOptions
                {
                    SubscriptionId = subscriptionId,
                    ResourceGroupName = resourceName,
                    UseExistingResourceGroup = UseExistingResourceGroup,
                    Region = Region.Region,
                };

                await NavigationService.NavigateToAsync<ResultsViewModel>();
            }
        }
        
        private void CalculateCanContinue()
        {
            CanContinue = UseExistingResourceGroup ? Region != null && ResourceGroup != null : newResourceGroupName.Validate() && Region != null;
        }

        private void UseExistingResourceGroupChanged()
        {
            newResourceGroupName.ValueChanged -= OnNewResourceGroupNameChanged;
            CalculateCanContinue();

            newResourceGroupName.Value = null;
            newResourceGroupName.ClearErrors();

            if (!UseExistingResourceGroup)
            {
                newResourceGroupName.ValueChanged += OnNewResourceGroupNameChanged;
            }
        }

        private void OnNewResourceGroupNameChanged(object sender, string groupName)
        {
            if (!UseExistingResourceGroup)
            {
                CalculateCanContinue();
            }
        }

        private bool CheckResourceGroupUnique(string groupName)
        {
            return !ResourceGroups.Any(r =>
                r.Name.Equals(groupName, StringComparison.InvariantCulture));
        }
    }
}