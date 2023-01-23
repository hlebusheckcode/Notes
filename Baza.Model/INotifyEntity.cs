using System.ComponentModel;

namespace Baza.Model
{
    public interface INotifyEntity : IEntity, INotifyPropertyChanged
    {
        public bool NotifyChanges { get; set; }
    }
}
