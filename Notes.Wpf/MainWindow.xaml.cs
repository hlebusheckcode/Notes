using Notes.Model;
using Notes.Controls.Commands;
using Notes.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Microsoft.Win32;
using System.IO;
using System.Text.Json;
using Notes.Wpf.Controls;
using System.Windows.Media;

namespace Notes
{
    public enum SearchMode
    {
        All,
        Headers,
        Contents
    }

    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private IMemoRepository _repository;
        private Memo? _currentItem;
        private string _filterText = string.Empty;
        private RemoveOption _removeOption = RemoveOption.WithoutRemoved;
        private bool _fullCreatedDate = false;
        private bool _fullUpdatedDate = false;
        private SearchMode _searchMode = SearchMode.All;

        public MainWindow(IMemoRepository repository)
        {
            InitializeComponent();
            SaveCommand = new RelayCommand(async (_) => await SaveCurrentItem(), (_) => CurrentItem != null);
            DataContext = this;
            _repository = repository;
            Load();
        }

        public ObservableCollection<Memo> Items { get; } = new ObservableCollection<Memo>();
        public Memo? CurrentItem
        {
            get => _currentItem;
            set
            {
                _currentItem = value;
                OnPropertyChanged(nameof(CurrentItem));
            }
        }

        public string FilterText
        {
            get => _filterText;
            set
            {
                _filterText = value;
                OnPropertyChanged(nameof(FilterText));
                var collectionView = CollectionViewSource.GetDefaultView(MemoList.ItemsSource);
                collectionView.Refresh();
            }
        }

        public ICommand SaveCommand { get; set; }
        public RemoveOption RemoveOption
        {
            get => _removeOption;
            set
            {
                _removeOption = value;
                var collectionView = CollectionViewSource.GetDefaultView(MemoList.ItemsSource);
                collectionView.Refresh();
            }
        }

        public bool FullCreatedDate
        {
            get => _fullCreatedDate;
            set
            {
                if (_fullCreatedDate != value)
                {
                    _fullCreatedDate = value;
                    OnPropertyChanged(nameof(FullCreatedDate));
                }
            }
        }
        public bool FullUpdatedDate
        {
            get => _fullUpdatedDate;
            set
            {
                if (_fullUpdatedDate != value)
                {
                    _fullUpdatedDate = value;
                    OnPropertyChanged(nameof(FullUpdatedDate));
                }
            }
        }

        public SearchMode SearchMode
        {
            get => _searchMode;
            set
            {
                _searchMode = value;
                var collectionView = CollectionViewSource.GetDefaultView(MemoList.ItemsSource);
                collectionView.Refresh();
            }
        }

        private async Task Load()
        {
            Items.Clear();
            var items = await _repository.Get();
            foreach (var item in items)
                Items.Add(item);
        }

        private void AddClick(object sender, RoutedEventArgs e)
            => Add();
        private void RemoveClick(object sender, RoutedEventArgs e)
            => RemoveCurrentItem();
        private void RecoverClick(object sender, RoutedEventArgs e)
            => RecoverCurrentItem();
        private void DeleteClick(object sender, RoutedEventArgs e)
            => DeleteCurrentItem();
        private void RefreshClick(object sender, RoutedEventArgs e)
            => Refresh();

        private void ImportClick(object sender, RoutedEventArgs e)
            => Import();
        private void ExportClick(object sender, RoutedEventArgs e)
            => Export();

        private void CloseFilterClick(object sender, RoutedEventArgs e)
            => ShowFilter.IsChecked = false;

        private void OpenConfigurationClick(object sender, RoutedEventArgs e)
            => new ConfigurationWindow(_repository) { Owner = this }.Show();

        private void SaveItemEvent(object sender, KeyboardFocusChangedEventArgs e)
            => SaveCurrentItem();

        public async void Refresh()
        {
            CurrentItem = null;
            await Load();
        }

        private void Add()
        {
            CurrentItem = new Memo();
        }

        private async Task SaveCurrentItem()
        {
            if (CurrentItem == null) return;

            if (CurrentItem.New && (!string.IsNullOrEmpty(CurrentItem.Header) || !string.IsNullOrEmpty(CurrentItem.Body)))
            {
                _ = await _repository.Insert(CurrentItem);
                CurrentItem.ApplyChanges();
                Items.Add(CurrentItem);
            }
            else if (CurrentItem.HasChanges || CurrentItem.BodyProperties.HasChanges)
            {
                _ = await _repository.Update(CurrentItem);
                CurrentItem.ApplyChanges();
            }

            OnPropertyChanged(nameof(CurrentItem));
        }

        private async Task RemoveCurrentItem()
        {
            if (CurrentItem == null) return;

            if (CurrentItem.Id == 0)
                CurrentItem = null;
            else
            {
                if (CustomMessageBox.Show(
                    this,
                    $"Remove {CurrentItem.Header}?",
                    "Warning",
                    "\u0028",
                    (SolidColorBrush)Application.Current.FindResource("WarningSolidBrush"),
                    MessageBoxButton.YesNo
                    ) == MessageBoxResult.Yes)
                    _ = await _repository.Remove(CurrentItem);
                else return;
            }

            await Load();
        }

        private async Task RecoverCurrentItem()
        {
            if (CurrentItem == null) return;

            if (CurrentItem.Id == 0)
                CurrentItem = null;
            else
                await _repository.Recover(CurrentItem);

            await Load();
        }

        private async Task DeleteCurrentItem()
        {
            if (CurrentItem == null) return;

            if (CurrentItem.Id == 0)
                CurrentItem = null;
            else
            {
                if (CustomMessageBox.Show(
                    this,
                    $"Delete {CurrentItem.Header}?",
                    "Warning",
                    "\u0045",
                    (SolidColorBrush)Application.Current.FindResource("ErrorSolidBrush"),
                    MessageBoxButton.YesNo
                    ) == MessageBoxResult.Yes)
                    _ = await _repository.Delete(CurrentItem);
                else return;
            }

            await Load();
        }

        private async Task Import()
        {
            var openFileDialog = new OpenFileDialog()
            {
                Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
                RestoreDirectory = true
            };

            if (openFileDialog.ShowDialog() == true)
            {
                var content = await File.ReadAllTextAsync(openFileDialog.FileName);
                if (string.IsNullOrEmpty(content))
                    return;
                var newItems = JsonSerializer.Deserialize<Memo[]>(content);
                if (newItems?.Any() != true)
                    return;
                await _repository.Import(newItems);
                Refresh();
                CustomMessageBox.Show(
                    this,
                    $"Import completed.",
                    "Informing",
                    "\u0027",
                    (SolidColorBrush)Application.Current.FindResource("InformationSolidBrush"),
                    MessageBoxButton.OK);
            }
        }

        private async Task Export()
        {
            var saveFileDialog = new SaveFileDialog()
            {
                Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
                RestoreDirectory = true,
                FileName = "Exported"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                var content = JsonSerializer.Serialize(Items.ToArray());
                await File.WriteAllTextAsync(saveFileDialog.FileName, content);
                CustomMessageBox.Show(
                    this,
                    $"Export completed.",
                    "Informing",
                    "\u0030",
                    (SolidColorBrush)Application.Current.FindResource("InformationSolidBrush"),
                    MessageBoxButton.OK);
            }
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        #endregion INotifyPropertyChanged

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            var collectionView = CollectionViewSource.GetDefaultView(MemoList.ItemsSource);
            collectionView.Filter += item =>
            {
                var memo = item as Memo;
                if (memo == null)
                    return false;

                if ((string.IsNullOrEmpty(FilterText)
                    || (SearchMode != SearchMode.Contents && DeepContains(memo.Header, FilterText))
                    || (SearchMode != SearchMode.Headers && memo.Body.Contains(FilterText, StringComparison.OrdinalIgnoreCase)))
                 && (RemoveOption == RemoveOption.All
                    || (RemoveOption == RemoveOption.WithoutRemoved && memo.Removed == false)
                    || (RemoveOption == RemoveOption.OnlyRemoved && memo.Removed == true)))
                    return true;

                return false;
            };

            collectionView.SortDescriptions.Clear();
            collectionView.SortDescriptions.Add(new SortDescription(nameof(Memo.Favorite), ListSortDirection.Descending));
            collectionView.SortDescriptions.Add(new SortDescription(nameof(Memo.InsertedDate), ListSortDirection.Descending));

            MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
        }

        private void ClearFilter(object sender, RoutedEventArgs e)
            => FilterText = string.Empty;

        private bool DeepContains(string content, string search)
        {
            if (content.Contains(search, StringComparison.OrdinalIgnoreCase)
                || content.Contains(Translate(search), StringComparison.OrdinalIgnoreCase))
                return true;

            return false;
        }

        private readonly BijectiveDictionary<char> _dictionary = new()
        {
            { 'q', 'й' },
            { 'w', 'ц' },
            { 'e', 'у' },
            { 'r', 'к' },
            { 't', 'е' },
            { 'y', 'н' },
            { 'u', 'г' },
            { 'i', 'ш' },
            { 'o', 'щ' },
            { 'p', 'з' },
            { '[', 'х' },
            { ']', 'ъ' },
            { 'a', 'ф' },
            { 's', 'ы' },
            { 'd', 'в' },
            { 'f', 'а' },
            { 'g', 'п' },
            { 'h', 'р' },
            { 'j', 'о' },
            { 'k', 'л' },
            { 'l', 'д' },
            { ';', 'ж' },
            { '\'', 'э' },
            { 'z', 'я' },
            { 'x', 'ч' },
            { 'c', 'с' },
            { 'v', 'м' },
            { 'b', 'и' },
            { 'n', 'т' },
            { 'm', 'ь' },
            { ',', 'б' },
            { '.', 'ю' }
        };
        private string Translate(string input)
        {
            var result = new StringBuilder();

            foreach (char symbol in input.ToLower())
            {
                char newSymbol = symbol;
                try
                {
                    newSymbol = _dictionary[symbol];
                }
                catch { }
                result.Append(newSymbol);
            }

            return result.ToString();
        }

        private void CreatedDateChangeMode(object sender, MouseButtonEventArgs e)
        {
            FullCreatedDate = !FullCreatedDate;
            if(FullCreatedDate)
            {
                var binding = ShortInsertedDateTextBlock.GetBindingExpression(VisibilityProperty);
                Binding bindingCopy = new Binding(binding.ParentBinding.Path.Path);
                bindingCopy.Converter = binding.ParentBinding.Converter;
                bindingCopy.ConverterParameter = binding.ParentBinding.ConverterParameter;
                bindingCopy.Mode = binding.ParentBinding.Mode;
                FullInsertedDateTextBlock.SetBinding(VisibilityProperty, bindingCopy);
            }
            else
            {
                FullInsertedDateTextBlock.Visibility = Visibility.Collapsed;
            }
        }
        private void UpdateDateChangeMode(object sender, MouseButtonEventArgs e)
        {
            FullUpdatedDate = !FullUpdatedDate;
            if (FullUpdatedDate)
            {
                var binding = ShortUpdatedDateTextBlock.GetBindingExpression(VisibilityProperty);
                Binding bindingCopy = new Binding(binding.ParentBinding.Path.Path);
                bindingCopy.Converter = binding.ParentBinding.Converter;
                bindingCopy.ConverterParameter = binding.ParentBinding.ConverterParameter;
                bindingCopy.Mode = binding.ParentBinding.Mode;
                FullUpdatedDateTextBlock.SetBinding(VisibilityProperty, bindingCopy);
            }
            else
            {
                FullUpdatedDateTextBlock.Visibility = Visibility.Collapsed;
            }
        }
    }

    class BijectiveDictionary<T> : Dictionary<T, T>
        where T : notnull
    {
        public new T this[T index]
        {
            get
            {
                if (ContainsKey(index))
                    return base[index];
                if (ContainsValue(index))
                    return this.FirstOrDefault(x => index.Equals(x.Value)).Key;
                throw new KeyNotFoundException("index");
            }
        }
    }
}
