using System;
using System.Threading.Tasks;
using VisualProvision.Resources;
using VisualProvision.Services.Dialog;
using Xamarin.Forms;

[assembly: Dependency(typeof(DialogService))]
namespace VisualProvision.Services.Dialog
{
    public class DialogService
    {
        public Task DisplayAlert(string title, string message)
        {
            return Application.Current.MainPage.DisplayAlert(title, message, Translations.Common_Ok);
        }

        public Task<bool> DisplayConfirmation(string title, string message)
        {
            return Application.Current.MainPage.DisplayAlert(
                title, 
                message,
                Translations.Common_Ok, 
                Translations.Common_Cancel);
        }
    }
}