using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace MyShop_WPF_Application.Converters
{
    class ToDataRowStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString();
        }

        public object ConvertBack(
        object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Int16.Parse(value.ToString());
        }
    }
}
