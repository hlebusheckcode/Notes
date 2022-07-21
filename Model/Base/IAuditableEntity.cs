namespace Model.Base
{
    public interface IAuditableEntity : IEntity
    {
        public DateTime InsertedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
