using NUnit.Framework;
using System;
using Xamarin.UITest;

namespace VisualProvision.UITest.Pages
{
    public abstract class BasePage
    {
        protected IApp App => AppManager.App;

        protected bool OnAndroid => AppManager.Platform == Platform.Android;

        protected bool OniOS => AppManager.Platform == Platform.iOS;

        protected abstract PlatformQuery Trait { get; }

        protected BasePage()
        {
            AssertOnPage(TimeSpan.FromSeconds(30));
            App.Screenshot("On " + this.GetType().Name);
        }

        protected void AssertOnPage(TimeSpan? timeout = default(TimeSpan?))
        {
            var message = "Unable to verify on page: " + this.GetType().Name;

            if (timeout == null)
            {
                Assert.IsNotEmpty(App.Query(Trait.Current), message);
            }
            else
            {
                Assert.DoesNotThrow(() => App.WaitForElement(Trait.Current, timeout: timeout), message);
            }
        }

        protected void WaitForPageToLeave(TimeSpan? timeout = default(TimeSpan?))
        {
            timeout = timeout ?? TimeSpan.FromSeconds(5);
            var message = "Unable to verify *not* on page: " + this.GetType().Name;

            Assert.DoesNotThrow(() => App.WaitForNoElement(Trait.Current, timeout: timeout), message);
        }
    }
}
