using Microsoft.Xaml.Behaviors;
using Notes.Controls.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Notes.Controls.Behaviors
{
    class SearchBehavior : Behavior<TextBox>
    {
        private TextBox? _textBox;
        private TextBox? _searchTextBox;
        private TextBlock? _matchesTextBlock;
        private Button _nextButton;
        private Button _previousButton;
        private Dictionary<int, int> _indexes = new();
        private int _currentMatch = 0;
        private int _caretIndex = 0;

        protected override void OnAttached()
        {
            _textBox = AssociatedObject;
            SetShowCommand(_textBox, new ActionCommand(Show));
            SetCloseCommand(_textBox, new ActionCommand(Close));
            _textBox.InputBindings.Add(new KeyBinding(GetShowCommand(_textBox), Key.F, ModifierKeys.Control));
            _textBox.Loaded += (_, _) =>
            {
                _searchTextBox = (TextBox)_textBox.Template.FindName("SearchTextBox", _textBox);
                _searchTextBox.GotKeyboardFocus += (_, _) => _searchTextBox.SelectAll();
                _matchesTextBlock = (TextBlock)_textBox.Template.FindName("MatchesCount", _textBox);
                _matchesTextBlock.Text = Matches;
                _nextButton = (Button)_textBox.Template.FindName("NextButton", _textBox);
                _previousButton = (Button)_textBox.Template.FindName("PreviousButton", _textBox);
                _nextButton.Click += (_, _) => GoToNext();
                _previousButton.Click += (_, _) => GoToPrevious();
                SetButtonEnabled(false);
            };
        }

        protected override void OnDetaching()
        {
            if (_searchTextBox != null)
                _searchTextBox.TextChanged -= Search;
        }

        public string Matches => $"{_currentMatch}/{_indexes.Count}";

        public static readonly DependencyProperty SearchVisibilityProperty =
            DependencyProperty.RegisterAttached(
                "SearchVisibility",
                typeof(Visibility),
                typeof(SearchBehavior),
                new FrameworkPropertyMetadata(Visibility.Collapsed, FrameworkPropertyMetadataOptions.Inherits));

        public static Visibility GetSearchVisibility(UIElement target) =>
            (Visibility)target.GetValue(SearchVisibilityProperty);

        public static void SetSearchVisibility(UIElement target, Visibility value) =>
            target.SetValue(SearchVisibilityProperty, value);

        public static readonly DependencyProperty ShowCommandProperty =
            DependencyProperty.RegisterAttached(
                "ShowCommand",
                typeof(ICommand),
                typeof(SearchBehavior),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));

        public static ICommand GetShowCommand(UIElement target) =>
            (ICommand)target.GetValue(ShowCommandProperty);

        public static void SetShowCommand(UIElement target, ICommand value) =>
            target.SetValue(ShowCommandProperty, value);

        public static readonly DependencyProperty CloseCommandProperty =
            DependencyProperty.RegisterAttached(
                "CloseCommand",
                typeof(ICommand),
                typeof(SearchBehavior),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));

        public static ICommand GetCloseCommand(UIElement target) =>
            (ICommand)target.GetValue(CloseCommandProperty);

        public static void SetCloseCommand(UIElement target, ICommand value) =>
            target.SetValue(CloseCommandProperty, value);

        private void Show()
        {
            if (_searchTextBox == null || _textBox == null) return;

            _caretIndex = _textBox.CaretIndex;
            SetSearchVisibility(Visibility.Visible);
            _searchTextBox.Text = _textBox.SelectedText;
            Search(null, null);
            Keyboard.Focus(_searchTextBox);
            _searchTextBox.TextChanged += Search;
        }

        private void Close()
        {
            if (_searchTextBox == null || _textBox == null) return;

            _searchTextBox.TextChanged -= Search;
            SetSearchVisibility(Visibility.Collapsed);
            if(string.IsNullOrEmpty(_textBox.SelectedText) && _textBox.CaretIndex == 0)
                _textBox.CaretIndex = _caretIndex;
        }

        private void SetSearchVisibility(Visibility visibility)
        {
            if (_textBox == null) return;

            SetSearchVisibility(_textBox, visibility);
        }

        private void Search(object sender, TextChangedEventArgs e)
        {
            if (_textBox == null || string.IsNullOrWhiteSpace(_textBox.Text)
                || _searchTextBox == null || string.IsNullOrWhiteSpace(_searchTextBox.Text))
            {
                if (_matchesTextBlock != null)
                    _matchesTextBlock.Text = "0/0";
                if (_textBox != null)
                {
                    _textBox.Select(0, 0);
                    _textBox.CaretIndex = _caretIndex;
                }
                SetButtonEnabled(false);
                return;
            }

            var indexes = AllIndexesOf(_textBox.Text, _searchTextBox.Text);
            if (indexes.Any() == false)
            {
                if (_matchesTextBlock != null)
                    _matchesTextBlock.Text = "0/0";
                if (_textBox != null)
                {
                    _textBox.Select(0, 0);
                    _textBox.CaretIndex = _caretIndex;
                }
                SetButtonEnabled(false);
                return;
            }

            _indexes = new(
                Enumerable.Range(1, indexes.Count)
                    .Select(i => new KeyValuePair<int, int>(i, indexes[i - 1])));
            _currentMatch = 0;

            if (_matchesTextBlock != null)
                _matchesTextBlock.Text = Matches;
            if (_indexes.Count > 0)
                SetButtonEnabled(true);
        }

        private void SetButtonEnabled(bool enabled)
        {
            _nextButton.IsEnabled = enabled;
            _previousButton.IsEnabled = enabled;
        }

        private void GoToNext()
        {
            _currentMatch++;
            if (_indexes.ContainsKey(_currentMatch) == false)
                _currentMatch = 1;
            GoToMatch(_currentMatch);
            if(_currentMatch == 1)
                GoToMatch(1);
        }

        private void GoToPrevious()
        {
            _currentMatch--;
            if (_currentMatch < 1)
                _currentMatch = _indexes.Count;
            GoToMatch(_currentMatch);
        }

        private void GoToMatch(int matchIndex)
        {
            _textBox.CaretIndex = _indexes[matchIndex];
            _textBox.Select(_textBox.CaretIndex, _searchTextBox.Text.Length);
            Keyboard.Focus(_textBox);

            if (_matchesTextBlock != null)
                _matchesTextBlock.Text = Matches;
        }

        static List<int> AllIndexesOf(string str, string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("the string to find may not be empty", nameof(value));
            List<int> indexes = new List<int>();
            for (int index = 0; ; index += value.Length)
            {
                index = str.IndexOf(value, index, StringComparison.OrdinalIgnoreCase);
                if (index == -1)
                    return indexes;
                indexes.Add(index);
            }
        }
    }
}
