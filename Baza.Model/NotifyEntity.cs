using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Baza.Model
{
    public class NotifyEntity : Entity, INotifyEntity
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        [NotMapped, JsonIgnore]
        public bool NotifyChanges { get; set; } = true;

        protected void OnPropertyChanged(string? propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        protected override bool SetValue<T>(ref T property, T value, string propertyName, Action<T>? action)
        {
            if (base.SetValue(ref property, value, propertyName, action))
            {
                if(NotifyChanges)
                    OnPropertyChanged(propertyName);
                return true;
            }
            return false;
        }
    }
}
