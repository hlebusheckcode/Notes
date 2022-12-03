using System.Windows.Controls;
using System.Windows;

namespace Notes.Controls.Helpers
{
    public class CheckBoxHelper
    {
        public static readonly DependencyProperty CheckedContentProperty =
            DependencyProperty.RegisterAttached(
                "CheckedContent",
                typeof(string),
                typeof(CheckBoxHelper),
                new FrameworkPropertyMetadata(default, FrameworkPropertyMetadataOptions.Inherits));

        public static string GetCheckedContent(CheckBox obj)
        {
            return (string)obj.GetValue(CheckedContentProperty);
        }

        public static void SetCheckedContent(CheckBox obj, string value)
        {
            obj.SetValue(CheckedContentProperty, value);
            obj.Checked += CheckChanged;
            obj.Unchecked += CheckChanged;
            obj.Tag = value;
        }

        private static void CheckChanged(object sender, RoutedEventArgs e)
        {
            if(sender is CheckBox checkBox)
            {
                var variable = checkBox.Content;
                checkBox.Content = checkBox.Tag as string;
                checkBox.Tag = variable;
            }
        }
    }
}
