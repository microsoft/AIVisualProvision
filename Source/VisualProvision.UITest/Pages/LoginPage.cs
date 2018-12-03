using System;
using System.Threading.Tasks;
using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;

namespace VisualProvision.UITest.Pages
{
    public class LoginPage : BasePage
    {
        private readonly Query clientIDField;
        private readonly Query tenantIDField;
        private readonly Query passwordField;
        private readonly Query loginButton;

        protected override PlatformQuery Trait => new PlatformQuery
        {
            Android = x => x.Marked("Client ID"),
            iOS = x => x.Marked("Client ID"),
        };

        public LoginPage()
        {
            clientIDField = x => x.Marked("entryClientID");
            tenantIDField = x => x.Marked("entryTenantID");
            passwordField = x => x.Marked("entryPassword");
            loginButton = x => x.Marked("buttonLogin");
        }

        public LoginPage EnterCredentials(string clientID, string tenantID, string password)
        {
            App.WaitForElement(clientIDField);

            App.ClearText(clientIDField);
            App.EnterText(clientIDField, clientID);

            App.DismissKeyboard();

            App.ClearText(tenantIDField);
            App.EnterText(tenantIDField, tenantID);

            App.DismissKeyboard();

            App.ClearText(passwordField);
            App.EnterText(passwordField, password);

            App.DismissKeyboard();

            App.Screenshot("Credentials Entered");

            return this;
        }

        public async Task SignIn()
        {
            App.Tap(loginButton);

            await Task.Delay(TimeSpan.FromSeconds(10));

            App.Screenshot("Login done");
        }
    }
}
