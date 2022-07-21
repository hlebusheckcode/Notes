using System.ComponentModel.DataAnnotations;

namespace Model.Base
{
    public class AuditableEntity : Entity, IAuditableEntity
    {
        private DateTime _insertedDate;
        private DateTime? _updatedDate;

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
    }
}
