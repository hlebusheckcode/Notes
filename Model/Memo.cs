using Model.Base;

namespace Model
{
    public class Memo : RemovableEntity
    {
        private string _header = string.Empty;
        private string _body = string.Empty;
        private bool _textWrapping = false;

        public string Header
        {
            get => _header;
            set => SetValue(ref _header, value, nameof(Header));
        }
        public string Body
        {
            get => _body;
            set => SetValue(ref _body, value, nameof(Body));
        }
        public bool TextWrapping
        {
            get => _textWrapping;
            set => SetValue(ref _textWrapping, value, nameof(TextWrapping));
        }
    }
}
