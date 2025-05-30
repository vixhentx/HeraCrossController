using HeraCrossController.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeraCrossController.Converters
{
    public class ConnectionStatusToTextColorConverter : IValueConverter
    {
        private static Color []color_table=[Colors.Red,Colors.Green,Colors.Blue,Colors.DarkRed];
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return (value is ConnectionStatusEnum v ? color_table[((int)v)] : Colors.Grey);
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
