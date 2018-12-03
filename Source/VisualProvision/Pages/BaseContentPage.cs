using VisualProvision.Utils.Localization;
using Xamarin.Forms;

namespace VisualProvision.Pages
{
    public class BaseContentPage : ContentPage
    {
        public bool ShowLogout { get; set; } = true;

        protected override void OnAppearing()
        {
            base.OnAppearing();

            AddToolbarItems();
        }

        public void ApplyTemplate()
        {
            ControlTemplate = Application.Current.Resources["PageTemplate"] as ControlTemplate;
        }

        private void AddToolbarItems()
        {
            ToolbarItems.Clear();

            if (ShowLogout)
            {
                var logoutItem = new ToolbarItem
                {
                    Icon = "ic_logout.png",
                    Text = VisualProvision.Resources.Translations.ActionBar_Item_Logout,
                    Command = new Command(() =>
                    {
                        (BindingContext as ViewModels.BaseViewModel).LogoutCommand.Execute(null);
                    }),
                };

                ToolbarItems.Add(logoutItem);
            }
        }
    }
}
