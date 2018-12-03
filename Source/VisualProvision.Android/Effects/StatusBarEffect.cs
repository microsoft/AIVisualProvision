using Android.Views;
using Plugin.CurrentActivity;
using VisualProvision.Droid.Effects;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportEffect(typeof(StatusBarEffect), nameof(StatusBarEffect))]
namespace VisualProvision.Droid.Effects
{
    public class StatusBarEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            var statusBarEffect = (VisualProvision.Effects.StatusBarEffect)Element.Effects.FirstOrDefault(e => e is VisualProvision.Effects.StatusBarEffect);

            if (statusBarEffect != null)
            {
                var backgroundColor = statusBarEffect.BackgroundColor.ToAndroid();
                Window currentWindow = GetCurrentWindow();
                currentWindow.SetStatusBarColor(backgroundColor);
            }
        }

        protected override void OnDetached()
        {
        }

        private Window GetCurrentWindow()
        {
            var window = CrossCurrentActivity.Current.Activity.Window;

            window.ClearFlags(WindowManagerFlags.TranslucentStatus);
            window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);

            return window;
        }
    }
}