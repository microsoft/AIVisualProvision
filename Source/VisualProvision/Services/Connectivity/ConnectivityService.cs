using VisualProvision.Services.Connectivity;
using Xamarin.Forms;

[assembly: Dependency(typeof(ConnectivityService))]
namespace VisualProvision.Services.Connectivity
{
    public class ConnectivityService
    {
        public ConnectivityService()
        {
            HasNetworkAccess = HasInternetNetworkAccess(Xamarin.Essentials.Connectivity.NetworkAccess);

            Xamarin.Essentials.Connectivity.ConnectivityChanged += OnConnectivityChanged;
        }

        public bool HasNetworkAccess { get; set; }

        private void OnConnectivityChanged(object sender, Xamarin.Essentials.ConnectivityChangedEventArgs e)
        {
            HasNetworkAccess = HasInternetNetworkAccess(e.NetworkAccess);
        }

        private bool HasInternetNetworkAccess(Xamarin.Essentials.NetworkAccess networkAccess)
        {
            return networkAccess == Xamarin.Essentials.NetworkAccess.Internet;
        }
    }
}