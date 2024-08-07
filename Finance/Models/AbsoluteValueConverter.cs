﻿using System.Globalization;
using System.Windows.Data;

namespace Finance.Models
{
    public class AbsoluteValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is float number)
            {
                return Math.Abs(number);
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is float number)
            {
                return number * -1;
            }
            return value;
        }
    }
}
