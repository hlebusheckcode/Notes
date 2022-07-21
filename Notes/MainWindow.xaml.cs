using Model;
using Repository;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace Notes
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private IMemoRepository _repository;
        private Memo? _currentItem;
        private string _filterText;

        public MainWindow(IMemoRepository repository)
        {
            InitializeComponent();
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

        private async Task Load()
        {
            Items.Clear();
            foreach (var item in await _repository.Get())
                Items.Add(item);
        }

        private void AddClick(object sender, RoutedEventArgs e)
            => Add();
        private void DeleteClick(object sender, RoutedEventArgs e)
            => DeleteCurrentItem();
        private void RefreshClick(object sender, RoutedEventArgs e)
            => Refresh();

        private void SaveItemEvent(object sender, KeyboardFocusChangedEventArgs e)
            => SaveCurrentItem();

        private async void Refresh()
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

            if (CurrentItem.Id == 0 && (!string.IsNullOrEmpty(CurrentItem.Header) || !string.IsNullOrEmpty(CurrentItem.Body)))
            {
                _ = await _repository.Insert(CurrentItem);
                CurrentItem.ApplyChanges();
                Items.Add(CurrentItem);
            }
            else if (CurrentItem.HasChanges)
            {
                _ = await _repository.Update(CurrentItem);
                CurrentItem.ApplyChanges();
            }

            OnPropertyChanged(nameof(CurrentItem));
        }

        private async Task DeleteCurrentItem()
        {
            if (CurrentItem == null) return;

            if(CurrentItem.Id == 0)
                CurrentItem = null;
            else
                _ = await _repository.Remove(CurrentItem);

            await Load();
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

                if (string.IsNullOrEmpty(FilterText)
                    || memo.Header.Contains(FilterText, StringComparison.OrdinalIgnoreCase)
                    || memo.Body.Contains(FilterText, StringComparison.OrdinalIgnoreCase))
                    return true;

                return false;
            };

            collectionView.SortDescriptions.Add(new SortDescription(nameof(Memo.InsertedDate), ListSortDirection.Descending));
        }

        private void ClearFilter(object sender, RoutedEventArgs e)
            => FilterText = string.Empty;
    }
}
