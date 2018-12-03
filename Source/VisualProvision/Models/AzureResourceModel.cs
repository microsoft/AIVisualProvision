using VisualProvision.Common;
using VisualProvision.Services.Management;
using System;

namespace VisualProvision.Models
{
    public class AzureResourceModel : BindableBase
    {
        private bool isCreated;
        private bool isDeploying;
        private bool hasFailed;
        private DateTime created;
        private float cost;

        public string Name { get; set; }

        public AzureResourceType Type { get; set; }

        public string Description { get; set; }

        public double EstimatedCost { get; set; }

        public DateTime Created
        {
            get => created;

            set => SetProperty(ref created, value);
        }

        public bool IsCreated
        {
            get => isCreated;

            set => SetProperty(ref isCreated, value);
        }

        public bool IsDeploying
        {
            get => isDeploying;

            set => SetProperty(ref isDeploying, value);
        }

        public bool HasFailed
        {
            get => hasFailed;

            set => SetProperty(ref hasFailed, value);
        }

        public float Cost
        {
            get => cost;

            set => SetProperty(ref cost, value);
        }

        public static AzureResourceModel FromResource(AzureResource resource)
        {
            return new AzureResourceModel
            {
                Name = resource.Name,
                Description = resource.Description,
                Type = resource.Type,
            };
        }
    }
}