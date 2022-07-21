using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Base
{
    public class RemovableEntity : AuditableEntity, IRemovableEntity
    {
        private DateTime? _removedDate;

        public DateTime? RemovedDate
        {
            get => _removedDate;
            set => SetValue(ref _removedDate, value, nameof(RemovedDate));
        }

        [NotMapped]
        public bool IsRemoved => RemovedDate.HasValue;
    }
}
