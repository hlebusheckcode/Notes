using Baza.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Notes.Model
{
    public class Memo : Entity, IAuditableEntity
    {
        private string _header = string.Empty;
        private string _body = string.Empty;
        private bool _textWrapping = false;
        private DateTime _insertedDate;
        private DateTime? _updatedDate;
        private DateTime? _removedDate;

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

        [Required]
        public DateTime InsertedDate
        {
            get => _insertedDate;
            set => SetValue(ref _insertedDate, value, nameof(InsertedDate));
        }
        public DateTime? UpdatedDate
        {
            get => _updatedDate;
            set => SetValue(ref _updatedDate, value, nameof(UpdatedDate));
        }
        public DateTime? RemovedDate
        {
            get => _removedDate;
            set => SetValue(ref _removedDate, value, nameof(RemovedDate));
        }

        [NotMapped, JsonIgnore]
        public bool IsRemoved => RemovedDate.HasValue;
    }
}
