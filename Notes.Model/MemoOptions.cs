using Notes.Model.Base;

namespace Notes.Model
{
    public class MemoOptions : Entity, IComparable, IComparable<MemoOptions>
    {
        private bool _favorite = false;
        private bool _wrapping = false;
        private bool _readOnly = false;

        public bool Favorite
        {
            get => _favorite;
            set => SetValue(ref _favorite, value);
        }

        public bool Wrapping
        {
            get => _wrapping;
            set => SetValue(ref _wrapping, value);
        }

        public bool ReadOnly
        {
            get => _readOnly;
            set => SetValue(ref _readOnly, value);
        }

        public int CompareTo(MemoOptions? other) => Favorite.CompareTo(other?.Favorite);

        private const string _compareTypeError = $"Object must be of type {nameof(MemoOptions)}";
        public int CompareTo(object? obj)
        {
            if (obj is MemoOptions other)
                return Favorite.CompareTo(other.Favorite);

            throw new ArgumentException(_compareTypeError);
        }
    }
}
