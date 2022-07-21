using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Base
{
    public abstract class Entity : IEntity
    {
        private Dictionary<string, object?>? _oldProperties;
        private int _id;

        [Key]
        public int Id
        {
            get => _id;
            set => SetValue(ref _id, value, nameof(Id));
        }

        [NotMapped]
        public bool TrackChanges { get; set; } = true;
        [NotMapped]
        public bool HasChanges => _oldProperties?.Any() == true;

        public void ApplyChanges()
            => _oldProperties = null;

        protected bool SetValue<T>(ref T property, T value, string propertyName)
        {
            if (Equals(property, value))
                return false;

            if(TrackChanges && _oldProperties?.ContainsKey(propertyName) != true)
                (_oldProperties ??= new Dictionary<string, object?>()).Add(propertyName, property);

            property = value;

            return true;
        }
    }
}
