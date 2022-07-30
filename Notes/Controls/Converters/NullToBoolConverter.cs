using System;
using System.Globalization;
using System.Windows.Data;

namespace Notes.Controls.Converters
{
    internal class NullToBoolConverter : IValueConverter
    {
        public static NullToBoolConverter Instance { get; } = new();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = false;

            if (value != null)
                result = true;

            if (parameter != null && parameter.ToString() == "invert")
                result = !result;

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
