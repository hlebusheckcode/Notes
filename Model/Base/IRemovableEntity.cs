namespace Model.Base
{
    public interface IRemovableEntity : IAuditableEntity
    {
        public DateTime? RemovedDate { get; set; }
        public bool IsRemoved { get; }
    }
}
