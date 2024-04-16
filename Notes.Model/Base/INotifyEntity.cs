using System.ComponentModel;

namespace Notes.Model.Base
{
    public interface INotifyEntity : IEntity, INotifyPropertyChanged
    {
        public bool NotifyChanges { get; set; }
    }
}
