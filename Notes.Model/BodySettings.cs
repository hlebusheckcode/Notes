using Baza.Models;

namespace Notes.Model
{
    public class BodySettings : Entity
    {
        private bool _canEdit = true;
        private bool _textWrapping = false;
        private int _fontSize = 12;

        public bool CanEdit
        {
            get => _canEdit;
            set => SetValue(ref _canEdit, value, nameof(CanEdit));
        }

        public bool TextWrapping
        {
            get => _textWrapping;
            set => SetValue(ref _textWrapping, value, nameof(TextWrapping));
        }

        public int FontSize
        {
            get => _fontSize;
            set => SetValue(ref _fontSize, value, nameof(FontSize));
        }
    }
}
