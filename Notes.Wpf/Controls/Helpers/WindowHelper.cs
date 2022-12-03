using System.Windows;
using System.Windows.Controls;

namespace Notes.Controls.Helpers
{
    public class WindowHelper
    {
        public static readonly DependencyProperty AdditionalButtonProperty =
            DependencyProperty.RegisterAttached(
                "AdditionalButton",
                typeof(Control),
                typeof(WindowHelper),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));

        public static Control GetAdditionalButton(Window obj)
        {
            return (Control)obj.GetValue(AdditionalButtonProperty);
        }

        public static void SetAdditionalButton(Window obj, Control value)
        {
            obj.SetValue(AdditionalButtonProperty, value);
        }
    }
}
