using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Threading.Tasks;
using Xamarin.Forms;
using static Microsoft.Azure.Management.Fluent.Azure;

namespace VisualProvision.Services.Management.Deployment
{
    public abstract class BaseDeployment
    {
        private readonly AnalyticsService analyticsService;
        private readonly Stopwatch watch;

        public BaseDeployment(IAuthenticated azure, DeploymentOptions options)
        {
            Azure = azure;
            Options = options;

            analyticsService = DependencyService.Resolve<AnalyticsService>();
            watch = new Stopwatch();
        }

        public IAuthenticated Azure { get; private set; }

        public DeploymentOptions Options { get; private set; }

        public async Task CreateAsync()
        {
            watch.Restart();

            await ExecuteCreateAsync();

            double totalSeconds = watch.Elapsed.TotalSeconds;
            Debug.WriteLine($"'{GetDeploymentName()}' created in {totalSeconds} seconds");

            analyticsService.TrackEvent(GetEventName(), new Dictionary<string, string>
            {
                { "ElapsedTime", totalSeconds.ToString(CultureInfo.InvariantCulture) },
            });

            watch.Stop();
        }

        protected abstract string GetEventName();

        protected abstract string GetDeploymentName();

        protected abstract Task ExecuteCreateAsync();
    }
}
