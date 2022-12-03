using System;
using System.Globalization;
using System.Windows.Data;

namespace Notes.Controls.Converters
{
    public class EnumToIntegerConverter : IValueConverter
    {
        public static EnumToIntegerConverter Instance = new();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int returnValue = 0;
            if (parameter is Type)
            {
                returnValue = (int)Enum.Parse((Type)parameter, value.ToString());
            }
            return returnValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Enum enumValue = default(Enum);
            if (parameter is Type)
            {
                enumValue = (Enum)Enum.Parse((Type)parameter, value.ToString());
            }
            return enumValue;
        }
    }
}
