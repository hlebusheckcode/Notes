using System;
using Xunit;

namespace Baza.Test.Model
{
    internal class MockEntity : Baza.Model.Entity
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

    public class Entity
    {
        [Fact]
        public void Entity_TrackChanges_Empty()
        {
            var entity = new MockEntity();

            Assert.False(entity.HasChanges);
        }

        [Fact]
        public void Entity_TrackChanges_Any()
        {
            var entity = new MockEntity();
            entity.Id = 1;

            Assert.True(entity.HasChanges);
        }

        [Fact]
        public void Entity_TrackChanges_Apply()
        {
            var entity = new MockEntity();
            entity.Id = 1;
            entity.ApplyChanges();

            Assert.False(entity.HasChanges);
        }

        [Fact]
        public void Entity_TrackChanges_Back()
        {
            var entity = new MockEntity();
            var oldId = entity.Id;
            entity.Id = 1;
            entity.RollbackChanges();

            Assert.Equal(oldId, entity.Id);
        }

        [Fact]
        public void Entity_TrackChanges_Off()
        {
            var entity = new MockEntity();
            entity.TrackChanges = false;
            entity.Id = 1;

            Assert.False(entity.HasChanges);
        }

        [Fact]
        public void Entity_TrackChanges_Off_Back()
        {
            var entity = new MockEntity();
            entity.TrackChanges = false;
            var newId = 1;
            entity.Id = newId;
            entity.RollbackChanges();

            Assert.Equal(newId, entity.Id);
        }

        [Fact]
        public void Entity_Identifier_IsNew()
        {
            var entity = new MockEntity();

            Assert.True(entity.New);
        }

        [Fact]
        public void Entity_Identifier_IsNotNew()
        {
            var entity = new MockEntity();
            entity.Id = 1;

            Assert.False(entity.New);
        }
    }
}
