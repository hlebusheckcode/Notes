namespace Repository.Base
{
    public interface IRepository<T>
        where T : class, new()
    {
        public Task<IEnumerable<T>> Get();
        public Task<T> Get(int id);

        public Task<T> Insert(T item);
        public Task<T> Update(T item);
        public Task<T> Update(int id, T item);
        public Task<T> Delete(T item);
        public Task<T> Delete(int id);
    }
}
