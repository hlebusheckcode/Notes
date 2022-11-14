using System.ComponentModel;

namespace Baza.Models
{
    public interface INotifyEntity : IEntity, INotifyPropertyChanged
    {
        public bool NotifyChanges { get; set; }
    }
}
