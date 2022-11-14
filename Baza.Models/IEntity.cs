namespace Baza.Models
{
    public interface IEntity
    {
        public int Id { get; set; }
        public bool IsNew { get; }
        public bool TrackChanges { get; set; }
        public bool HasChanges { get; }

        public void ApplyChanges();
        public void RollbackChanges();
    }
}
