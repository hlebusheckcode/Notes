using Baza.Models;

namespace Baza.Repositories
{
    public abstract class Repository<T> : IRepository<T>
        where T : IEntity, new()
    {

        public virtual Task<IEnumerable<T>> Get() => throw new NotImplementedException();
        public virtual Task<T> Get(int id) => throw new NotImplementedException();

        public virtual Task<T> Insert(T item) => throw new NotImplementedException();

        public virtual Task<T> Update(T item) => throw new NotImplementedException();
        public virtual Task<T> Update(int id, T item) => throw new NotImplementedException();

        public virtual Task<T> Delete(T item) => throw new NotImplementedException();
        public virtual Task<T> Delete(int id) => throw new NotImplementedException();
    }

    public abstract class Repository<T, TFilter> : Repository<T>, IRepository<T, TFilter>
        where T : IEntity, new()
        where TFilter : IEntity, new()
    {
        public virtual Task<IEnumerable<T>> Get(TFilter filter) => throw new NotImplementedException();
        public virtual Task<T> Get(int id, TFilter filter) => throw new NotImplementedException();

        public virtual Task<T> Insert(T item, TFilter filter) => throw new NotImplementedException();

        public virtual Task<T> Update(T item, TFilter filter) => throw new NotImplementedException();
        public virtual Task<T> Update(int id, T item, TFilter filter) => throw new NotImplementedException();

        public virtual Task<T> Delete(T item, TFilter filter) => throw new NotImplementedException();
        public virtual Task<T> Delete(int id, TFilter filter) => throw new NotImplementedException();
    }
}
