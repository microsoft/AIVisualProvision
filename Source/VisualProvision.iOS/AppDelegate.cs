using Foundation;
using Lottie.Forms.iOS.Renderers;
using UIKit;
using Xamarin.Forms.Platform.iOS;
using XColor = Xamarin.Forms.Color;

namespace VisualProvision.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new App());

            AnimationViewRenderer.Init();

            UiCustomizations();

            #if ENABLE_TEST_CLOUD
            Xamarin.Calabash.Start();
            #endif

            return base.FinishedLaunching(app, options);
        }

        private static void UiCustomizations()
        {
            XColor? xfSwitchColor = App.Current.Resources["LightBlue"] as XColor?;

            if (xfSwitchColor.HasValue)
            {
                UISwitch.Appearance.OnTintColor = xfSwitchColor.Value.ToUIColor();
            }
        }
    }
}