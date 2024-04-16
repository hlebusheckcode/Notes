using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace Notes.Model.Base
{
    public abstract class Entity : IEntity
    {
        #region Private variables

        private Dictionary<string, object?>? _oldProperties;

        #endregion Private variables

        #region Public properties

        [NotMapped, JsonIgnore]
        public bool New
        {
            get
            {
                var identifier = GetIdentifier();
                if (identifier == null)
                    return true;

                var type = identifier.GetType();
                if (type.IsValueType && Equals(identifier, Activator.CreateInstance(type)))
                    return true;

                return false;
            }
        }

        [NotMapped, JsonIgnore]
        public bool TrackChanges { get; set; } = true;

        [NotMapped, JsonIgnore]
        public IEnumerable<string> ChangedProperties =>
            _oldProperties?.Keys ?? Enumerable.Empty<string>();

        [NotMapped, JsonIgnore]
        public bool HasChanges => _oldProperties?.Count > 0;

        #endregion Public properties

        #region Public methods

        public virtual object? GetIdentifier()
            => throw new NotImplementedException();
        public virtual void SetIdentifier(object identifier)
            => throw new NotImplementedException();

        public virtual void ApplyChanges()
            => _oldProperties = null;
        public void RollbackChanges()
        {
            if(HasChanges)
            {
                var type = GetType();
                foreach (var property in _oldProperties!)
                    type.GetProperty(property.Key)?.SetValue(this, property.Value, null);
            }
        }

        #endregion Public methods

        #region Protected methods

        protected virtual bool SetValue<T>(ref T property, T value, [CallerMemberName] string? propertyName = null)
            => SetValue(ref property, value, null, propertyName);
        protected virtual bool SetValue<T>(ref T property, T value, Action<T>? action, [CallerMemberName] string? propertyName = null)
        {
            if (propertyName == null)
                throw new ArgumentNullException(nameof(propertyName));

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

        #endregion Protected methods
    }
}
