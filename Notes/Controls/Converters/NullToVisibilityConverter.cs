using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Notes.Controls.Converters
{
    internal class NullToVisibilityConverter : IValueConverter
    {
        public static NullToVisibilityConverter Instance { get; } = new();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = Visibility.Collapsed;

            if (value != null)
                result = Visibility.Visible;

            if (parameter != null && parameter.ToString() == "invert")
                result = result == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
