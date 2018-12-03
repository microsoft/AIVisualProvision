using System.Linq;
using Foundation;
using UIKit;
using VisualProvision.iOS.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportEffect(typeof(StatusBarEffect), nameof(StatusBarEffect))]
namespace VisualProvision.iOS.Effects
{
    public class StatusBarEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            var statusBarEffect = (VisualProvision.Effects.StatusBarEffect)Element.Effects.FirstOrDefault(e => e is VisualProvision.Effects.StatusBarEffect);

            if (statusBarEffect != null)
            {
                UIView statusBar = UIApplication.SharedApplication.ValueForKey(new NSString("statusBar")) as UIView;
              
                if (statusBar.RespondsToSelector(new ObjCRuntime.Selector("setBackgroundColor:")))
                {
                    statusBar.BackgroundColor = statusBarEffect.BackgroundColor.ToUIColor();
                }
            }
        }

        protected override void OnDetached()
        {
        }
    }
}