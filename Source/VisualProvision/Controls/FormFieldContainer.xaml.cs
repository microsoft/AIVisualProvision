using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace VisualProvision.Controls
{
    public partial class FormFieldContainer
    {
        public static readonly BindableProperty LabelTextProperty =
            BindableProperty.Create(nameof(LabelText), typeof(string), typeof(FormFieldContainer), Label.TextProperty.DefaultValue);

        public static readonly BindableProperty LabelStyleProperty =
            BindableProperty.Create(nameof(LabelStyle), typeof(Style), typeof(FormFieldContainer), StyleProperty.DefaultValue);

        public static readonly BindableProperty FieldViewProperty =
            BindableProperty.Create(nameof(FieldView), typeof(View), typeof(FormFieldContainer), default(View), propertyChanged: FieldViewChanged);

        public static readonly BindableProperty StatusViewProperty =
            BindableProperty.Create(nameof(StatusView), typeof(View), typeof(FormFieldContainer), default(View), propertyChanged: StatusViewChanged);

        public FormFieldContainer()
        {
            InitializeComponent();
        }

        public string LabelText
        {
            get { return (string)GetValue(LabelTextProperty); }
            set { SetValue(LabelTextProperty, value); }
        }

        public Style LabelStyle
        {
            get { return (Style)GetValue(LabelStyleProperty); }
            set { SetValue(LabelStyleProperty, value); }
        }

        public View FieldView
        {
            get { return (View)GetValue(FieldViewProperty); }
            set { SetValue(FieldViewProperty, value); }
        }

        public View StatusView
        {
            get { return (View)GetValue(StatusViewProperty); }
            set { SetValue(StatusViewProperty, value); }
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (FieldView != null)
            {
                FieldView.BindingContext = BindingContext;
            }

            if (StatusView != null)
            {
                StatusView.BindingContext = BindingContext;
            }
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == IsEnabledProperty.PropertyName)
            {
                UpdateIsEnabled();
            }
        }

        private static void FieldViewChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var container = bindable as FormFieldContainer;

            if (container.FieldView != null)
            {
                container.FieldView.BindingContext = container.BindingContext;
            }
        }

        private static void StatusViewChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var container = bindable as FormFieldContainer;

            if (container.StatusView != null)
            {
                container.StatusView.BindingContext = container.BindingContext;
            }
        }

        private void UpdateIsEnabled()
        {
            topLabel.Opacity = IsEnabled ? 0.4 : 0.2;
        }
    }
}