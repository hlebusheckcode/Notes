using Notes.Model;
using Notes.Model.Base;
using Notes.Repository;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

namespace Notes.ViewModel
{
    public class MemosViewModel : NotifyEntity
    {
        private readonly IMemoRepository _repository;
        private bool _loaded;
        private bool _collectionChanged;
        private ObservableCollection<Memo> _items = null!;
        private ICollectionView _itemsView = null!;
        private Memo? _currentItem;
        private string? _filterText;

        public MemosViewModel(IMemoRepository repository)
        {
            _repository = repository;
        }

        public ObservableCollection<Memo> Items
        {
            get => GetItems();
            set
            {
                SetValue(ref _items, value);
                _collectionChanged = true;
                OnPropertyChanged(nameof(ItemsView));
            }
        }

        public ICollectionView ItemsView => GetCollectionView();

        public Memo? CurrentItem
        {
            get => _currentItem;
            set => SetValue(ref _currentItem, value);
        }

        public string? FilterText
        {
            get => _filterText;
            set
            {
                SetValue(ref _filterText, value);
                _itemsView.Refresh();
            }
        }

        private async Task Load()
        {
            _items = new(await _repository.Get());
            _loaded = true;
        }

        private ObservableCollection<Memo> GetItems()
        {
            if (!_loaded)
                Load().Wait();

            return _items;
        }

        private ICollectionView GetCollectionView()
        {
            if (_collectionChanged || !_loaded)
            {
                _itemsView = new CollectionView(Items);
                _collectionChanged = false;

                _itemsView.Filter += item =>
                {
                    if (item is Memo memo)
                    {
                        return string.IsNullOrEmpty(FilterText) ||
                            memo.Header.Contains(FilterText) ||
                            memo.Body.Contains(FilterText);
                    }
                    return false;
                };
            }

            return _itemsView;
        }
    }
}
