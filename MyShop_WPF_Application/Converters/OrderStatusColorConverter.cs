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
    public class OrderStatusColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var status = (string)value;

            switch (status)
            {
                case "Mới tạo":
                    return Brushes.AliceBlue;
                case "Ðã thanh toán":
                    return Brushes.Green;
                case "Đã hủy":
                    return Brushes.Red;
                default:
                    return Brushes.DarkOliveGreen;
            }
        }

        public object ConvertBack(
        object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
