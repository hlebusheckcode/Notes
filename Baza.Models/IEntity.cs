namespace Baza.Models
{
    public interface IEntity
    {
        public bool IsNew { get; }
        public bool TrackChanges { get; set; }
        public bool HasChanges { get; }

        public object? GetIdentifier();
        public void SetIdentifier(object identifier);

        public void ApplyChanges();
        public void RollbackChanges();
    }
}
