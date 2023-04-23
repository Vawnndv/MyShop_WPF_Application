using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MyShop_WPF_Application.Converters
{
    class ToListViewStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString()!;
        }

        public object ConvertBack(
        object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString()!;
        }
    }
}
