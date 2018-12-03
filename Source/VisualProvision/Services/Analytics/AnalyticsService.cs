using System.Collections.Generic;
using Microsoft.AppCenter.Analytics;
using VisualProvision.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(AnalyticsService))]
namespace VisualProvision.Services
{
    public class AnalyticsService
    {
        public void TrackEvent(string name, Dictionary<string, string> properties = null)
        {
            Analytics.TrackEvent(name, properties);
        }
    }
}