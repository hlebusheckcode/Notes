using Repository.Base;

namespace SqliteRepository.Base
{
    public abstract class RemovableRepository<T> : Repository<T>, IRemovableRepository<T>
        where T : class, new()
    {
        public RemovableRepository(DataContext dataContext)
            : base(dataContext) { }

        public virtual Task<T> Recover(T item) => throw new NotImplementedException();
        public virtual Task<T> Recover(int id) => throw new NotImplementedException();
        public virtual Task<T> Remove(T item) => throw new NotImplementedException();
        public virtual Task<T> Remove(int id) => throw new NotImplementedException();
    }
}
