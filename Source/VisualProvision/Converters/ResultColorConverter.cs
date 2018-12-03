using System;
using System.Globalization;
using Xamarin.Forms;

namespace VisualProvision.Converters
{
    public class ResultColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isLoading = (bool)value;

            return isLoading ? Color.White : (object)Color.FromHex("#32323b");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
