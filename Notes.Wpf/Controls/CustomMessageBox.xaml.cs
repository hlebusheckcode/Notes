using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Notes.Wpf.Controls
{
    public partial class CustomMessageBox : Window
    {
        private static CustomMessageBox? _messageBox;
        private static MessageBoxResult _result = MessageBoxResult.None;

        public CustomMessageBox()
        {
            InitializeComponent();
        }

        public static MessageBoxResult Show(Window owner, string content, string title, string icon, Color iconBrush, MessageBoxButton buttons)
            => Show(owner, content, title, icon, new SolidColorBrush(iconBrush), buttons);
        public static MessageBoxResult Show(Window owner, string content, string title, string icon, SolidColorBrush iconBrush, MessageBoxButton buttons)
        {
            _messageBox = new CustomMessageBox();
            _messageBox.Owner = owner;
            _messageBox.ContentTextBlock.Text = content;
            _messageBox.Title = title;
            _messageBox.IconTextBlock.Text = icon;
            _messageBox.IconTextBlock.Foreground = iconBrush;
            ConfigureButtons(_messageBox, buttons);
            _messageBox.ShowDialog();
            return _result;
        }

        private static void ConfigureButtons(CustomMessageBox messageBox, MessageBoxButton buttons)
        {
            switch (buttons)
            {
                case MessageBoxButton.OK:
                    messageBox.OkButton.Visibility = Visibility.Visible;
                    messageBox.OkButton.Focus();
                    break;
                case MessageBoxButton.OKCancel:
                    messageBox.OkButton.Visibility = Visibility.Visible;
                    messageBox.CancelButton.Visibility = Visibility.Visible;
                    messageBox.CancelButton.Focus();
                    break;
                case MessageBoxButton.YesNo:
                    messageBox.YesButton.Visibility = Visibility.Visible;
                    messageBox.NoButton.Visibility = Visibility.Visible;
                    messageBox.NoButton.Focus();
                    break;
                case MessageBoxButton.YesNoCancel:
                    messageBox.YesButton.Visibility = Visibility.Visible;
                    messageBox.NoButton.Visibility = Visibility.Visible;
                    messageBox.CancelButton.Visibility = Visibility.Visible;
                    messageBox.CancelButton.Focus();
                    break;
                default: break;
            }
        }

        private void ResultClick(object sender, RoutedEventArgs e)
        {
            if (Equals(sender, OkButton))
                _result = MessageBoxResult.OK;
            else if (Equals(sender, YesButton))
                _result = MessageBoxResult.Yes;
            else if (Equals(sender, NoButton))
                _result = MessageBoxResult.No;
            else if (Equals(sender, CancelButton))
                _result = MessageBoxResult.Cancel;
            else
                _result = MessageBoxResult.None;

            Close();
        }

        private void LoadedWindow(object sender, RoutedEventArgs e)
        {
            var minButton = (Button)Template.FindName("MinButton", this);
            var maxButton = (Button)Template.FindName("MaxButton", this);
            minButton.Visibility = Visibility.Collapsed;
            maxButton.Visibility = Visibility.Collapsed;
            ResizeMode = ResizeMode.NoResize;
        }
    }
}
