using System;
using Xamarin.UITest;
using Xamarin.UITest.Utils;

namespace VisualProvision.UITest
{
    internal static class AppManager
    {
        private static IApp app;

        public static IApp App
        {
            get
            {
                if (app == null)
                {
                    throw new NullReferenceException("'AppManager.App' not set. Call 'AppManager.StartApp()' before trying to access it.");
                }

                return app;
            }
        }

        private static Platform? platform;

        public static Platform Platform
        {
            get
            {
                if (platform == null)
                {
                    throw new NullReferenceException("'AppManager.Platform' not set.");
                }

                return platform.Value;
            }

            set
            {
                platform = value;
            }
        }

        public static void StartApp()
        {
            if (Platform == Platform.Android)
            {
                app = ConfigureApp
                    .Android
                    .WaitTimes(new WaitTimes())
                    .EnableLocalScreenshots()
                    
                    // Used to run a .apk file:
                    .ApkFile("../../../VisualProvision.Android/bin/UITestConfig/com.microsoft.aiprovision.apk")
                    .StartApp();
            }

            if (Platform == Platform.iOS)
            {
                app = ConfigureApp
                    .iOS
                    .Debug()
                    .WaitTimes(new WaitTimes())

                    // Used to run a .app file on an ios simulator:
                    // .AppBundle("path/to/file.app")
                    // Used to run a .ipa file on a physical ios device:
                    .InstalledApp("com.microsoft.aiprovision")
                    .StartApp();
            }
        }
    }

    public class WaitTimes : IWaitTimes
    {
        public TimeSpan GestureCompletionTimeout => TimeSpan.FromSeconds(30);

        public TimeSpan GestureWaitTimeout => TimeSpan.FromSeconds(30);

        public TimeSpan WaitForTimeout => TimeSpan.FromSeconds(30);
    }
}