using Baza.Model;
using Baza.Repository;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Baza.ViewModel
{
    public class ItemsViewModel<T> : NotifyEntity
        where T : IEntity, new()
    {
        #region Private variables

        private readonly List<T> _itemsCash;
        private T? _currentItem;
        private string _filterText = string.Empty;
        private ICommand? _createCommand;

        #endregion Private variables

        #region Ctor

        public ItemsViewModel(IRepository<T> repository)
        {
            Repository = repository;
            _itemsCash = new List<T>(20);
            Items = new ObservableCollection<T>(_itemsCash);
            Task.Run(Load);
        }

        #endregion Ctor

        #region Public properties

        public ObservableCollection<T> Items { get; }

        public T? CurrentItem
        {
            get => _currentItem;
            set => SetValue(ref _currentItem, value);
        }

        public Func<T, bool>? Filtration { get; protected set; }

        public string FilterText
        {
            get => _filterText;
            set
            {
                SetValue(ref _filterText, value);
                Filter();
            }
        }

        #endregion Public properties

        protected IRepository<T> Repository { get; }

        #region Public methods

        public async Task Load()
        {
            Items.Clear();
            var items = await Repository.Get();
            foreach (var item in items)
                Items.Add(item);
        }

        #endregion Public methods

        private void Filter()
        {
            if (Filtration == null)
                return;

            if(_itemsCash.Any())
            {
                for (int i = 0; i < _itemsCash.Count; i++)
                {
                    if (Filtration(_itemsCash[i]))
                    {
                        Items.Add(_itemsCash[i]);
                        _itemsCash.RemoveAt(i);
                        i--;
                    }
                }
            }

            if(Items.Any())
            {
                for (int i = 0; i < Items.Count; i++)
                {
                    if (!Filtration(Items[i]))
                    {
                        _itemsCash.Add(Items[i]);
                        Items.RemoveAt(i);
                        i--;
                    }
                }
            }
        }
    }
}
