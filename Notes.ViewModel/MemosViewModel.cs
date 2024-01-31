using Baza.Model;
using Notes.Model;
using Notes.Repository;

namespace Notes.ViewModel
{
    public class MemosViewModel : NotifyEntity
    {
        private Memo? _currentItem;
        private string? _searchText;

        public MemosViewModel(IMemoRepository repository)
        {
            Repository = repository;
            Filtering = DefaultFilter;
            Load().Wait();
        }

        public IEnumerable<Memo> ItemsSorce { get; private set; } = null!;

        public IEnumerable<Memo> Items
            => string.IsNullOrEmpty(SearchText)
            ? ItemsSorce
            : ItemsSorce;

        public Memo? CurrentItem
        {
            get => _currentItem;
            set => SetValue(ref _currentItem, value);
        }

        public string? SearchText
        {
            get => _searchText;
            set
            {
                SetValue(ref _searchText, value);
                OnPropertyChanged(nameof(Items));
            }
        }

        private IMemoRepository Repository { get; }

        private Func<Memo, bool> Filtering { get; set; }

        private async Task Load()
        {
            ItemsSorce = await Repository.Get();
        }

        private bool DefaultFilter(Memo item)
        {
            return true;
        }
    }
}
