using System;
using System.ComponentModel;
using Android.Content.Res;
using Android.Widget;
using VisualProvision.Droid.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportEffect(typeof(DisplayPlaceholderAsTextEffect), nameof(DisplayPlaceholderAsTextEffect))]
namespace VisualProvision.Droid.Effects
{
    public class DisplayPlaceholderAsTextEffect : PlatformEffect
    {
        private EditText text;

        protected override void OnAttached()
        {
            text = Control as EditText;

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
                text.SetHintTextColor(GetTextColorFromElement().ToAndroid());
            }
        }

        private Color GetTextColorFromElement()
        {
            if (Element is Picker picker)
            {
                return picker.TextColor;
            }

            return Color.Default;
        }
    }
}