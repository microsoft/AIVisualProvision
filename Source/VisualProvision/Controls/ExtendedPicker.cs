using Xamarin.Forms;

namespace VisualProvision.Controls
{
    public class ExtendedPicker : Picker
    {
        public static BindableProperty PlaceholderColorProperty =
            BindableProperty.Create(nameof(PlaceholderColor), typeof(Color), typeof(ExtendedPicker), Color.Black, BindingMode.TwoWay);

        public Color PlaceholderColor
        {
            get { return (Color)GetValue(PlaceholderColorProperty); }
            set { SetValue(PlaceholderColorProperty, value); }
        }
    }
}