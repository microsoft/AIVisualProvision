using Foundation;
using UIKit;
using VisualProvision.Controls;
using VisualProvision.iOS.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using static VisualProvision.iOS.Helpers.LineLayerHelper;

[assembly: ExportRenderer(typeof(ExtendedPicker), typeof(ExtendedPickerRenderer))]
namespace VisualProvision.iOS.Renderers
{
    public class ExtendedPickerRenderer : PickerRenderer
    {
        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            LineLayer lineLayer = GetOrAddLineLayer(Control);
            ApplyFrame(lineLayer, Frame, Control);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                if (Control != null)
                {
                    Control.BorderStyle = UITextBorderStyle.None;
                }

                UpdateLineColor();
                UpdateTitleColor();
            }
        }

        private void UpdateLineColor()
        {
            LineLayer lineLayer = GetOrAddLineLayer(Control);
            lineLayer.BorderColor = Color.White.ToCGColor();
        }

        private void UpdateTitleColor()
        {
            var customPicker = Element as ExtendedPicker;
            Color placeholderColor = customPicker.PlaceholderColor;
            UIColor color = placeholderColor.ToUIColor();

            var placeholderAttributes = new NSAttributedString(customPicker.Title, new UIStringAttributes()
            {
                ForegroundColor = color,
            });

            Control.AttributedPlaceholder = placeholderAttributes;
        }
    }
}
