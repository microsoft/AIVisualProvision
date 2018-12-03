using Android.Content;
using VisualProvision.Controls;
using VisualProvision.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ExtendedPicker), typeof(ExtendedPickerRenderer))]
namespace VisualProvision.Droid.Renderers
{
    public class ExtendedPickerRenderer : PickerRenderer
    {
        public ExtendedPickerRenderer(Context context)
            : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || e.NewElement != null)
            {
                var customPicker = e.NewElement as ExtendedPicker;
                Control.SetHintTextColor(customPicker.PlaceholderColor.ToAndroid());
            }
        }
    }
}