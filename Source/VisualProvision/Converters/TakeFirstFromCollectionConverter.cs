using System;
using System.Collections;
using System.Globalization;
using Xamarin.Forms;

namespace VisualProvision.Converters
{
    public class TakeFirstFromCollectionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is ICollection collection))
            {
                return value;
            }

            if (collection.Count == 0)
            {
                return null;
            }

            IEnumerator enumerator = collection.GetEnumerator();
            enumerator.MoveNext();

            return enumerator.Current;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
