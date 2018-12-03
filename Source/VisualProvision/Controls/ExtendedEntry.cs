using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace VisualProvision.Controls
{
    public class ExtendedEntry : Entry
    {
        public static readonly BindableProperty LineColorProperty =
            BindableProperty.Create(nameof(LineColor), typeof(Color), typeof(ExtendedEntry), Color.Default, propertyChanged: LineColorChanged);

        public static readonly BindableProperty FocusLineColorProperty =
            BindableProperty.Create(nameof(FocusLineColor), typeof(Color), typeof(ExtendedEntry), Color.Default);

        public static readonly BindableProperty IsValidProperty =
            BindableProperty.Create(nameof(IsValid), typeof(bool), typeof(ExtendedEntry), true);

        public static readonly BindableProperty InvalidLineColorProperty =
            BindableProperty.Create(nameof(InvalidLineColor), typeof(Color), typeof(ExtendedEntry), Color.Default);

        private Color lineColorToApply;

        public ExtendedEntry()
        {
            Focused += OnFocused;
            Unfocused += OnUnfocused;

            ResetLineColor();
        }

        public Color LineColorToApply
        {
            get => this.lineColorToApply;

            private set
            {
                lineColorToApply = value;
                OnPropertyChanged(nameof(LineColorToApply));
            }
        }

        public Color LineColor
        {
            get { return (Color)GetValue(LineColorProperty); }
            set { SetValue(LineColorProperty, value); }
        }

        public Color FocusLineColor
        {
            get { return (Color)GetValue(FocusLineColorProperty); }
            set { SetValue(FocusLineColorProperty, value); }
        }

        public bool IsValid
        {
            get { return (bool)GetValue(IsValidProperty); }
            set { SetValue(IsValidProperty, value); }
        }

        public Color InvalidLineColor
        {
            get { return (Color)GetValue(InvalidLineColorProperty); }
            set { SetValue(InvalidLineColorProperty, value); }
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == IsValidProperty.PropertyName)
            {
                CheckValidity();
            }
        }

        private static void LineColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var entry = bindable as ExtendedEntry;
            entry.ResetLineColor();
        }

        private void OnFocused(object sender, FocusEventArgs e)
        {
            IsValid = true;
            LineColorToApply = FocusLineColor != Color.Default
                ? FocusLineColor
                : GetNormalStateLineColor();
        }

        private void OnUnfocused(object sender, FocusEventArgs e)
        {
            ResetLineColor();
        }

        private void ResetLineColor()
        {
            LineColorToApply = GetNormalStateLineColor();
        }

        private void CheckValidity()
        {
            if (!IsValid)
            {
                LineColorToApply = InvalidLineColor;
            }
        }

        private Color GetNormalStateLineColor()
        {
            return LineColor != Color.Default
                    ? LineColor
                    : TextColor;
        }
    }
}
