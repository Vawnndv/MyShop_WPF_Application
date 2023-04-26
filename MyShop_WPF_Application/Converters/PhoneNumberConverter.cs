using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MyShop_WPF_Application.Converters
{
    public class PhoneNumberConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string phoneNumber)
            {
                // Remove any non-digit characters from the input string
                phoneNumber = new string(phoneNumber.Where(char.IsDigit).ToArray());

                // Format the phone number using the pattern "###-###-####"
                if (phoneNumber.Length == 10)
                {
                    return phoneNumber.Substring(0, 3) + "-" + phoneNumber.Substring(3, 3) + "-" + phoneNumber.Substring(6);
                }
            }

            return value;
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
