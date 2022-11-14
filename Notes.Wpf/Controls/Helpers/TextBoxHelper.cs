using Notes.Controls.Commands;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Notes.Controls.Helpers
{
    class TextBoxHelper
    {
        public static RelayCommand ClearTextBoxCommand { get; } = new RelayCommand(ClearTextBox);

        private static void ClearTextBox(object o)
        {
            if (o is TextBox textBox)
            {
                textBox.Clear();
                Keyboard.Focus(textBox);
            }
        }

        public static readonly DependencyProperty ClearTextButtonProperty =
            DependencyProperty.RegisterAttached(
                "ClearTextButton",
                typeof(bool),
                typeof(TextBoxHelper),
                new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.Inherits));

        public static readonly DependencyProperty WatermarkProperty =
            DependencyProperty.RegisterAttached(
                "Watermark",
                typeof(string),
                typeof(TextBoxHelper),
                new FrameworkPropertyMetadata(default, FrameworkPropertyMetadataOptions.Inherits));

        public static bool GetClearTextButton(DependencyObject obj)
        {
            return (bool)obj.GetValue(ClearTextButtonProperty);
        }

        public static void SetClearTextButton(DependencyObject obj, bool value)
        {
            obj.SetValue(ClearTextButtonProperty, value);
        }

        public static string GetWatermark(DependencyObject obj)
        {
            return (string)obj.GetValue(WatermarkProperty);
        }

        public static void SetWatermark(DependencyObject obj, string value)
        {
            obj.SetValue(WatermarkProperty, value);
        }
    }
}
