using Baza.Model;

namespace Notes.Model
{
    public class TextProperties : Entity
    {
        private bool _wrapping = false;
        private bool _readOnly = false;

        public bool Wrapping
        {
            get => _wrapping;
            set => SetValue(ref _wrapping, value, nameof(Wrapping));
        }

        public bool ReadOnly
        {
            get => _readOnly;
            set => SetValue(ref _readOnly, value, nameof(ReadOnly));
        }
    }
}
