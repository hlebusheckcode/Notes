using System;
using System.Windows.Data;

namespace Notes.Controls.Converters
{
    public class EnumToBoolConverter : IValueConverter
    {
        public static EnumToBoolConverter Instance { get; } = new();

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return Equals(value, parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value?.Equals(true) == true ? parameter : Binding.DoNothing;
        }
    }
}
