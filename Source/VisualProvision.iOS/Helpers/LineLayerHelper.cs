using CoreAnimation;
using CoreGraphics;
using System;
using System.Linq;
using UIKit;

namespace VisualProvision.iOS.Helpers
{
    internal static class LineLayerHelper
    {
        public static LineLayer GetOrAddLineLayer(UIView view)
        {
            var lineLayer = view.Layer.Sublayers?.OfType<LineLayer>().FirstOrDefault();

            if (lineLayer == null)
            {
                lineLayer = new LineLayer();

                view.Layer.AddSublayer(lineLayer);
                view.Layer.MasksToBounds = true;
            }

            return lineLayer;
        }

        public static void ApplyFrame(LineLayer layer, CGRect frame, UIView view)
        {
            layer.Frame = new CGRect(
                0,
                frame.Size.Height - LineLayer.DefaultLineHeight,
                view.Frame.Size.Width,
                LineLayer.DefaultLineHeight);
        }

        internal class LineLayer : CALayer
        {
            private static nfloat lineHeight = 2f;

            public LineLayer()
            {
                BorderWidth = lineHeight;
            }

            public static nfloat DefaultLineHeight => lineHeight;
        }
    }
}