using System.ComponentModel;
using UIKit;
using VisualProvision.iOS.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportEffect(typeof(DisplayPlaceholderAsTextEffect), "DisplayPlaceholderAsTextEffect")]
namespace VisualProvision.iOS.Effects
{
    public class DisplayPlaceholderAsTextEffect : PlatformEffect
    {
        private UITextField text;

        protected override void OnAttached()
        {
            text = Control as UITextField;

            UpdatePlaceholderTextColor();
        }

        protected override void OnDetached()
        {
            text = null;
        }

        protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnElementPropertyChanged(args);

            if (args.PropertyName == Picker.TextColorProperty.PropertyName)
            {
                UpdatePlaceholderTextColor();
            }
        }

        private void UpdatePlaceholderTextColor()
        {
            if (text != null)
            {
                text.TextColor = GetTextColorFromElement().ToUIColor();
            }
        }

        private Color GetTextColorFromElement()
        {
            return Element is Picker picker ? picker.TextColor : Color.Default;
        }
    }
}