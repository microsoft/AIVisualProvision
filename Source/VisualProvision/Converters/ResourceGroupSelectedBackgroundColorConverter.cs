using System;
using System.Globalization;
using Xamarin.Forms;

namespace VisualProvision.Converters
{
    public class ResourceGroupSelectedBackgroundColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is bool ? (bool)value ? Color.FromHex("#69a6ed") : (object)Color.Transparent : Color.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}