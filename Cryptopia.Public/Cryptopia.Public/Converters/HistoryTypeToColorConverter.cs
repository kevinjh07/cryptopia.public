
using Cryptopia.Public.Models;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace Cryptopia.Public.Converters
{
    public class HistoryTypeToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((HistoryType)value)
            {
                case HistoryType.Buy:
                    return Color.Green;
                case HistoryType.Sell:
                    return Color.Red;
                default:
                    return Color.Default;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
