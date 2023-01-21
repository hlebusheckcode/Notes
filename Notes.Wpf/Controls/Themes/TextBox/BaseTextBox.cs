using System;
using System.Windows;
using System.Windows.Input;

namespace Notes.Wpf.Controls.Themes.TextBox
{
    internal static class LocalExtensions
    {
        public static void ForTextBoxFromTemplate(this object templateFrameworkElement, Action<System.Windows.Controls.TextBox> action)
        {
            if (((FrameworkElement)templateFrameworkElement).TemplatedParent is System.Windows.Controls.TextBox textBox)
                action(textBox);
        }
    }

    public partial class BaseTextBox
    {
        void CutClick(object sender, RoutedEventArgs args) => sender.ForTextBoxFromTemplate(tb => tb.Cut());
        void CopyClick(object sender, RoutedEventArgs args) => sender.ForTextBoxFromTemplate(tb => tb.Copy());
        void PasteClick(object sender, RoutedEventArgs args) => sender.ForTextBoxFromTemplate(tb => tb.Paste());
        void SelectAllClick(object sender, RoutedEventArgs args) => sender.ForTextBoxFromTemplate(tb => tb.SelectAll());
        void ClearClick(object sender, RoutedEventArgs args) => sender.ForTextBoxFromTemplate(tb => tb.Clear());

        void SelectLineClick(object sender, MouseButtonEventArgs e)
        {
            if(e.ClickCount != 3)
                return;

            sender.ForTextBoxFromTemplate(tb =>
            {
                int lineIndex = tb.GetLineIndexFromCharacterIndex(tb.CaretIndex);
                int lineStartingCharIndex = tb.GetCharacterIndexFromLineIndex(lineIndex);
                int lineLength = tb.GetLineLength(lineIndex);
                tb.Select(lineStartingCharIndex, lineLength);
            });
        }

        void ContextMenuOpened(object sender, RoutedEventArgs args)
        {
            sender.ForTextBoxFromTemplate(tb =>
            {
                
            });

            //if (SelectedText == "")
            //    ContextMenuItemCopy.IsEnabled = ContextMenuItemCut.IsEnabled = false;
            //else
            //    ContextMenuItemCopy.IsEnabled = ContextMenuItemCut.IsEnabled = true;

            //if (Clipboard.ContainsText())
            //    ContextMenuItemPaste.IsEnabled = true;
            //else
            //    ContextMenuItemPaste.IsEnabled = false;
        }
    }
}
