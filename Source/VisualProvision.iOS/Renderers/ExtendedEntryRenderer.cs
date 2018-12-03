using VisualProvision.Controls;
using VisualProvision.iOS.Helpers;
using VisualProvision.iOS.Renderers;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using static VisualProvision.iOS.Helpers.LineLayerHelper;

[assembly: ExportRenderer(typeof(ExtendedEntry), typeof(ExtendedEntryRenderer))]
namespace VisualProvision.iOS.Renderers
{
    public class ExtendedEntryRenderer : EntryRenderer
    {
        public ExtendedEntry ExtendedEntryElement => this.Element as ExtendedEntry;

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            LineLayer lineLayer = LineLayerHelper.GetOrAddLineLayer(this.Control);
            LineLayerHelper.ApplyFrame(lineLayer, this.Frame, this.Control);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                if (this.Control != null)
                {
                    this.Control.BorderStyle = UIKit.UITextBorderStyle.None;
                }

                this.UpdateLineColor();
                this.UpdateCursorColor();
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName.Equals(nameof(ExtendedEntry.LineColorToApply)))
            {
                UpdateLineColor();
                UpdateCursorColor();
            }
            else if (e.PropertyName.Equals(Entry.TextColorProperty.PropertyName))
            {
                UpdateCursorColor();
            }
        }

        private void UpdateLineColor()
        {
            LineLayer lineLayer = LineLayerHelper.GetOrAddLineLayer(this.Control);
            lineLayer.BorderColor = ExtendedEntryElement.LineColorToApply.ToCGColor();
        }

        private void UpdateCursorColor()
        {
            this.Control.TintColor = ExtendedEntryElement.LineColorToApply.ToUIColor();
        }
    }
}