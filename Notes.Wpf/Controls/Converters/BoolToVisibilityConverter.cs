using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Notes.Controls.Converters
{
    internal class BoolToVisibilityConverter : IValueConverter
    {
        public static BoolToVisibilityConverter Instance { get; } = new();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool visible = (value as bool?) == true;

            if (parameter != null && parameter.ToString() == "invert")
                visible = !visible;

            if(visible)
                return Visibility.Visible;
            else
                return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
