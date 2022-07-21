using Repository.Base;

namespace SqliteRepository.Base
{
    public abstract class Repository<T> : IRepository<T>
        where T : class, new()
    {
        protected DataContext _dataContext;

        public Repository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public virtual Task<IEnumerable<T>> Get() => throw new NotImplementedException();

        public virtual Task<T> Get(int id) => throw new NotImplementedException();
        public virtual Task<T> Insert(T item) => throw new NotImplementedException();
        public virtual Task<T> Update(T item) => throw new NotImplementedException();
        public virtual Task<T> Update(int id, T item) => throw new NotImplementedException();
        public virtual Task<T> Delete(T item) => throw new NotImplementedException();
        public virtual Task<T> Delete(int id) => throw new NotImplementedException();

    }
}
