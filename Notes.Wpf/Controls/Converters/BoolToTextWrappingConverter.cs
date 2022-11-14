using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Notes.Controls.Converters
{
    internal class BoolToTextWrappingConverter : IValueConverter
    {
        public static BoolToTextWrappingConverter Instance { get; } = new();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool wrapping = (value as bool?) == true;

            if (parameter != null && parameter.ToString() == "invert")
                wrapping = !wrapping;

            if (wrapping)
                return TextWrapping.Wrap;
            else
                return TextWrapping.NoWrap;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            TextWrapping wrapping = (value as TextWrapping?) ?? TextWrapping.NoWrap;
            bool result = wrapping == TextWrapping.Wrap;

            if (parameter != null && parameter.ToString() == "invert")
                result = !result;

            return result;
        }
    }
}
