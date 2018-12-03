namespace VisualProvision.UITest.Pages
{
    public class SubscriptionPage : BasePage
    {
        protected override PlatformQuery Trait => new PlatformQuery
        {
            Android = x => x.Marked("buttonContinue"),
            iOS = x => x.Marked("buttonContinue"),
        };
    }
}
