using VisualProvision.Controls;
using VisualProvision.Droid.Renderers;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ExtendedEntry), typeof(ExtendedEntryRenderer))]
namespace VisualProvision.Droid.Renderers
{
    public class ExtendedEntryRenderer : EntryRenderer
    {
        public ExtendedEntryRenderer(Android.Content.Context context)
            : base(context)
        {
        }

        public ExtendedEntry ExtendedEntryElement => Element as ExtendedEntry;

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                Control.InputType |= Android.Text.InputTypes.TextFlagNoSuggestions;
                UpdateLineColor();
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName.Equals(nameof(ExtendedEntry.LineColorToApply)))
            {
                UpdateLineColor();
            }
        }

        private void UpdateLineColor()
        {
            Control?.Background?.SetColorFilter(ExtendedEntryElement.LineColorToApply.ToAndroid(), Android.Graphics.PorterDuff.Mode.SrcAtop);
        }
    }
}