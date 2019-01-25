using System.Threading.Tasks;
using NUnit.Framework;
using VisualProvision.UITest.Pages;
using Xamarin.UITest;

namespace VisualProvision.UITest
{
    public class Tests : BaseTestFixture
    {
        public Tests(Platform platform) 
            : base(platform)
        {
        }

        // In order to run this test, please change "TestSettings" constants, indicating a valid configuration
        /*
        [Test]
        public async Task SuccessSignInTestAsync()
        {
            await new LoginPage()
                .EnterCredentials(TestSettings.ValidClientId, TestSettings.ValidTenantId, TestSettings.ValidPwd)
                .SignIn();
            new SubscriptionPage();
        }
        */

        [Test]
        public async Task FailedSignInTestAsync()
        {
            await new LoginPage()
                .EnterCredentials(TestSettings.BadClientID, TestSettings.BadTenantID, TestSettings.BadPassword)
                .SignIn();
        }
    }
}
