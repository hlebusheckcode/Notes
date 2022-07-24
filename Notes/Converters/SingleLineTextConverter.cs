using System;
using System.Windows.Data;

namespace Notes.Converters
{
    public class SingleLineTextConverter : IValueConverter
    {
        public static SingleLineTextConverter Instance { get; } = new();

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string text = (string)value;
            text = text.Replace(Environment.NewLine, " ");
            return text;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
