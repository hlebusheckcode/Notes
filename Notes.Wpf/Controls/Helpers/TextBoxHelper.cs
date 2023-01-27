using Notes.Controls.Commands;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Notes.Controls.Helpers
{
    class TextBoxHelper
    {
        #region ClearTextButton

        public static readonly DependencyProperty ClearTextButtonProperty =
            DependencyProperty.RegisterAttached(
                "ClearTextButton",
                typeof(bool),
                typeof(TextBoxHelper),
                new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.Inherits));

        public static RelayCommand ClearTextBoxCommand { get; } = new RelayCommand(ClearTextBox);

        private static void ClearTextBox(object o)
        {
            if (o is TextBox textBox)
            {
                textBox.Clear();
                Keyboard.Focus(textBox);
            }
        }

        public static bool GetClearTextButton(UIElement obj)
        {
            return (bool)obj.GetValue(ClearTextButtonProperty);
        }

        public static void SetClearTextButton(UIElement obj, bool value)
        {
            if(obj is TextBox textBox)
            {
                obj.SetValue(ClearTextButtonProperty, value);
                //if (value)
                //    Attach(textBox);
                //else
                //    Detach(textBox);
            }
        }

        private static void Attach(TextBox textBox)
        {
            var scrollViewer = (ScrollViewer)textBox.Template.FindName("PART_ContentHost", textBox);
            var contentGrid = (Grid)textBox.Template.FindName("InnerGrid", textBox);
            var clearButton = BuildClearButton(textBox);
            if (contentGrid.ColumnDefinitions.Count == 0)
                contentGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            contentGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            Grid.SetRow(clearButton, Grid.GetRow(scrollViewer));
            Grid.SetColumn(clearButton, contentGrid.ColumnDefinitions.Count - 1);
            contentGrid.Children.Add(clearButton);
        }

        private static void Detach(TextBox textBox)
        {
            throw new NotImplementedException();
        }

        private static Button BuildClearButton(TextBox textBox)
        {
            FrameworkElementFactory button = new FrameworkElementFactory(typeof(Button));
            button.SetValue(Button.StyleProperty, Application.Current.FindResource("LabelTemplate"));
            button.SetValue(Button.VisibilityProperty, new TemplateBindingExtension(ClearTextButtonProperty));
            button.SetValue(Button.CommandProperty, ClearTextBoxCommand);
            button.SetValue(Button.CommandParameterProperty, textBox);
            var result = new Button();
            result.Template.VisualTree = button;
            return result;
        }

        #endregion ClearTextButton

        public static readonly DependencyProperty WatermarkProperty =
            DependencyProperty.RegisterAttached(
                "Watermark",
                typeof(string),
                typeof(TextBoxHelper),
                new FrameworkPropertyMetadata(default, FrameworkPropertyMetadataOptions.Inherits));

        public static string GetWatermark(TextBox obj)
        {
            return (string)obj.GetValue(WatermarkProperty);
        }

        public static void SetWatermark(TextBox obj, string value)
        {
            obj.SetValue(WatermarkProperty, value);
        }
    }
}
