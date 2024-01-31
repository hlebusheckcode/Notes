using Baza.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Notes.Model
{
    public class Memo : Entity, IAuditableEntity
    {
        #region Private variables

        private int _id;
        private string _header = string.Empty;
        private string _body = string.Empty;
        private bool _favorite = false;
        private DateTime _insertedDate;
        private DateTime? _updatedDate;
        private DateTime? _removedDate;

        #endregion Private variables

        #region Public properties

        [Key, JsonIgnore]
        public int Id
        {
            get => _id;
            set => SetValue(ref _id, value, nameof(Id));
        }

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
        public bool Favorite
        {
            get => _favorite;
            set => SetValue(ref _favorite, value, nameof(Favorite));
        }

        public TextProperties BodyProperties { get; set; } = new();

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
        public bool Removed => RemovedDate.HasValue && RemovedDate < DateTime.Now;

        #endregion Public properties

        #region Public methods

        public override object? GetIdentifier() => Id;
        public override void SetIdentifier(object identifier) => Id = Convert.ToInt32(identifier);

        public override void ApplyChanges()
        {
            base.ApplyChanges();
            BodyProperties.ApplyChanges();
        }

        public override string ToString() => Header;

        #endregion Public properties
    }
}
