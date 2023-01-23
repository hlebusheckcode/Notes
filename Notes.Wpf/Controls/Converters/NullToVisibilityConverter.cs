using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace Notes.Controls.Converters
{
    internal class NullToVisibilityConverter : IValueConverter
    {
        public static NullToVisibilityConverter Instance { get; } = new();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var parameters = parameter?.ToString()?.Split(' ')?.Select(s => s.ToLower());
            var visible = false;

            if (Equals(value, null) == false)
                visible = true;

            if (parameters?.Contains("invert") == true)
                visible = false;

            return 
                visible ? 
                Visibility.Visible : 
                    parameters?.Contains("hidden") == true ? 
                    Visibility.Hidden : 
                    Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
