using Xamarin.Forms;

namespace VisualProvision.Pages
{
    public partial class CustomNavigationPage : NavigationPage
    {
        public CustomNavigationPage()
            : base()
        {
            InitializeComponent();
        }

        public CustomNavigationPage(Page root)
            : base(root)
        {
            InitializeComponent();
        }
    }
}