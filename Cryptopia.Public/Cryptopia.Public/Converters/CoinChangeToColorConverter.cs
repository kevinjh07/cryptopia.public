using System;
using System.Globalization;
using Xamarin.Forms;

namespace Cryptopia.Public.Converters
{
    public class CoinChangeToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double change = 0;
            double.TryParse((value == null) ? string.Empty : value.ToString(), out change);
            return (change >= 0) ? Color.Green : Color.Red;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
