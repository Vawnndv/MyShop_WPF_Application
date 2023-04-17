using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MyShop_WPF_Application.Converters
{
    public class PriceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double doubleValue)
            {
                int price = (int)Math.Round(doubleValue);
                return price.ToString("N0", culture) + " VND";
            }
            else if (value is int intValue)
            {
                return intValue.ToString("N0", culture) + " VND";
            }
            else
            {
                return "0 VND";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
