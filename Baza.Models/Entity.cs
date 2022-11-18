using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Baza.Models
{
    public abstract class Entity : IEntity
    {
        private Dictionary<string, object?>? _oldProperties;

        [NotMapped, JsonIgnore]
        public bool IsNew => GetIdentifier() == null;
        [NotMapped, JsonIgnore]
        public bool TrackChanges { get; set; } = true;
        [NotMapped, JsonIgnore]
        public bool HasChanges => _oldProperties?.Any() == true;

        public virtual object? GetIdentifier()
            => throw new NotImplementedException();
        public virtual void SetIdentifier(object identifier)
            => throw new NotImplementedException();

        public void ApplyChanges()
            => _oldProperties = null;
        public void RollbackChanges()
        {
            if(HasChanges)
            {
                var type = GetType();
                foreach (var property in _oldProperties)
                    type.GetProperty(property.Key).SetValue(this, property.Value, null);
            }
        }

        protected bool SetValue<T>(ref T property, T value, string propertyName)
            => SetValue(ref property, value, propertyName, null);
        protected virtual bool SetValue<T>(ref T property, T value, string propertyName, Action<T>? action)
        {
            if (Equals(property, value))
                return false;

            if(TrackChanges && _oldProperties?.ContainsKey(propertyName) != true)
                (_oldProperties ??= new Dictionary<string, object?>())
                    .Add(propertyName, property);
            else if(TrackChanges && Equals(_oldProperties?[propertyName], value))
            {
                _oldProperties?.Remove(propertyName);
                _oldProperties = null;
            }

            property = value;

            action?.Invoke(value);

            return true;
        }
    }
}
