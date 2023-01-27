using Baza.Model;

namespace Baza.Repository
{
    public interface IRepository<T>
        where T : IEntity, new()
    {
        public Task<IEnumerable<T>> Get();
        public Task<T> Get(int id);

        public Task<T> Insert(T item);
        public Task<T> Update(T item);
        public Task<T> Update(int id, T item);
        public Task<T> Delete(T item);
        public Task<T> Delete(int id);
    }

    public interface IRepository<T, TFilter> : IRepository<T>
        where T : IEntity, new()
        where TFilter : IEntity, new()
    {
        public Task<IEnumerable<T>> Get(TFilter filter);
        public Task<T> Get(int id, TFilter filter);

        public Task<T> Insert(T item, TFilter filter);
        public Task<T> Update(T item, TFilter filter);
        public Task<T> Update(int id, T item, TFilter filter);
        public Task<T> Delete(T item, TFilter filter);
        public Task<T> Delete(int id, TFilter filter);
    }
}
