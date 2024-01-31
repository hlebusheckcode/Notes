using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
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

        protected override bool SetValue<T>(ref T property, T value, Action<T>? action, [CallerMemberName] string propertyName = null!)
        {
            if (base.SetValue(ref property, value, action, propertyName))
            {
                if(NotifyChanges)
                    OnPropertyChanged(propertyName);
                return true;
            }
            return false;
        }
    }
}
