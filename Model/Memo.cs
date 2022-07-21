using Model.Base;

namespace Model
{
    public class Memo : RemovableEntity
    {
        private string _header = string.Empty;
        private string _body = string.Empty;

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
    }
}
