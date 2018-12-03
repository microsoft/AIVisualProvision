using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Support.V7.App;
using System.Threading.Tasks;

namespace VisualProvision.Droid
{
    [Activity(
        Label = "AI VisualProvision",
        Icon = "@mipmap/icon",
        Theme = "@style/SplashTheme",
        MainLauncher = true,
        NoHistory = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
        ScreenOrientation = ScreenOrientation.Portrait)]
    public class SplashActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            StartActivity(new Intent(Application.Context, typeof(MainActivity))
                 .AddFlags(ActivityFlags.NoAnimation));
        }
    }
}