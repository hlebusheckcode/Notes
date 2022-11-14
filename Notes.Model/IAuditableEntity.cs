namespace Notes.Model
{
    public interface IAuditableEntity
    {
        public DateTime InsertedDate  { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? RemovedDate { get; set; }
    }
}
