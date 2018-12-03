using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VisualProvision.Resources;
using VisualProvision.Services.Dialog;
using Xamarin.Forms;

namespace VisualProvision.Utils
{
    public static class AppSettingsValidator
    {
        private static Regex alphaNumericRegex = new Regex("^[a-zA-Z0-9]*$", RegexOptions.Compiled);

        public static async Task<bool> CheckSettingsAsync()
        {
            var dialogService = DependencyService.Get<DialogService>();

            if (Device.RuntimePlatform == Device.Android)
            {
                if (!IsGuid(AppSettings.AppCenterAndroid))
                {
                    await dialogService.DisplayAlert(
                        Translations.AppSettings_InvalidSetting_Title,
                        GetInvalidMessage(nameof(AppSettings.AppCenterAndroid)));

                    return false;
                }
            }
            else if (Device.RuntimePlatform == Device.iOS)
            {
                if (!IsGuid(AppSettings.AppCenterIos))
                {
                    await dialogService.DisplayAlert(
                        Translations.AppSettings_InvalidSetting_Title,
                        GetInvalidMessage(nameof(AppSettings.AppCenterIos)));

                    return false;
                }
            }

            if (!IsUrl(AppSettings.CustomVisionPredictionUrl))
            {
                await dialogService.DisplayAlert(
                    Translations.AppSettings_InvalidSetting_Title,
                    GetInvalidMessage(nameof(AppSettings.CustomVisionPredictionUrl)));

                return false;
            }

            if (!IsAlphaNumeric(AppSettings.CustomVisionPredictionKey))
            {
                await dialogService.DisplayAlert(
                    Translations.AppSettings_InvalidSetting_Title,
                    GetInvalidMessage(nameof(AppSettings.CustomVisionPredictionKey)));

                return false;
            }

            if (!IsAlphaNumeric(AppSettings.ComputerVisionKey))
            {
                await dialogService.DisplayAlert(
                    Translations.AppSettings_InvalidSetting_Title,
                    GetInvalidMessage(nameof(AppSettings.ComputerVisionKey)));

                return false;
            }

            return true;
        }

        public static bool IsAlphaNumeric(string setting)
        {
            return alphaNumericRegex.IsMatch(setting);
        }

        public static bool IsGuid(string setting)
        {
            Guid guid;

            return Guid.TryParse(setting, out guid);
        }

        public static bool IsUrl(string setting)
        {
            Uri uri;

            return Uri.TryCreate(setting, UriKind.Absolute, out uri);
        }

        private static string GetInvalidMessage(string settingName)
        {
            try
            {
                return string.Format(
                    Translations.AppSettings_InvalidSetting_Message,
                    settingName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
