using System;
using Xunit;

namespace Baza.Test.Model
{
    internal class MockNotifyEntity : Baza.Model.NotifyEntity
    {
        private int _id;

        public int Id
        {
            get => _id;
            set => SetValue(ref _id, value, nameof(Id));
        }

        public override object? GetIdentifier() => Id;
        public override void SetIdentifier(object identifier) => Id = Convert.ToInt32(identifier);
    }

    public class NotifyEntity
    {
        [Theory]
        [InlineData(true, nameof(MockNotifyEntity.Id))]
        [InlineData(false, default(string))]
        public void NotifyEntity_NotifyChanges(bool notifyChanges, string expectedPropertyName)
        {
            var notifiedPropertyName = default(string);
            void WriteName(object? obj, System.ComponentModel.PropertyChangedEventArgs e)
                => notifiedPropertyName = e.PropertyName;

            var entity = new MockNotifyEntity { NotifyChanges = notifyChanges };
            entity.PropertyChanged += WriteName;
            entity.Id = 1;
            entity.PropertyChanged -= WriteName;

            Assert.Equal(expectedPropertyName, notifiedPropertyName);
        }
    }
}
