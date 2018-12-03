using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using VisualProvision.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace VisualProvision
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new Pages.CustomNavigationPage(new ContentPage());
        }

        protected override async void OnStart()
        {
            base.OnStart();
            ConfigureAppCenter();
            ServicesRegistrations();

            var navigationService = DependencyService.Resolve<NavigationService>();
            await navigationService.InitializeAsync();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        private static void ConfigureAppCenter()
        {
#if !DEBUG
            AppCenter.Start($"ios={AppSettings.AppCenterIos};android={AppSettings.AppCenterAndroid}", typeof(Analytics), typeof(Crashes));
#endif
        }

        private static void ServicesRegistrations()
        {
                DependencyService.Register<Services.Management.AzureService>();
                DependencyService.Register<Services.Recognition.RecognitionService>();
                DependencyService.Register<Services.Management.Deployment.DeploymentManager>();
        }
    }
}