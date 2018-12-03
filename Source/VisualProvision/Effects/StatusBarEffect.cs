using Xamarin.Forms;

namespace VisualProvision.Effects
{
    public class StatusBarEffect : RoutingEffect
    {
        public Color BackgroundColor { get; set; }

        public StatusBarEffect()
            : base($"{nameof(VisualProvision)}.{nameof(StatusBarEffect)}")
        {
        }
    }
}