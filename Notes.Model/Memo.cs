using Notes.Model.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Notes.Model
{
    public class Memo : Entity
    {
        #region Private variables

        private uint _id;
        private string _header = string.Empty;
        private string _body = string.Empty;
        private DateTime _insertedDate;
        private DateTime _updatedDate;
        private DateTime? _removedDate;

        #endregion Private variables

        #region Public properties

        [Key, JsonIgnore]
        public uint Id
        {
            get => _id;
            set => SetValue(ref _id, value);
        }

        public string Header
        {
            get => _header;
            set => SetValue(ref _header, value);
        }
        public string Body
        {
            get => _body;
            set => SetValue(ref _body, value);
        }

        public MemoOptions Options { get; set; } = new();

        [Required]
        public DateTime InsertedDate
        {
            get => _insertedDate;
            set => SetValue(ref _insertedDate, value);
        }
        [Required]
        public DateTime UpdatedDate
        {
            get => _updatedDate;
            set => SetValue(ref _updatedDate, value);
        }
        public DateTime? RemovedDate
        {
            get => _removedDate;
            set => SetValue(ref _removedDate, value, nameof(RemovedDate));
        }

        [NotMapped, JsonIgnore]
        public bool Removed => RemovedDate.HasValue && RemovedDate < DateTime.Now;

        #endregion Public properties

        #region Public methods

        public override object? GetIdentifier() => Id;
        public override void SetIdentifier(object identifier) => Id = Convert.ToUInt32(identifier);

        public override void ApplyChanges()
        {
            Options.ApplyChanges();
            base.ApplyChanges();
        }

        public override string ToString() => Header;

        #endregion Public properties
    }
}
