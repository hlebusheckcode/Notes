namespace Repository.Base
{
    public interface IRemovableRepository<T> : IRepository<T>
        where T : class, new()
    {
        public Task<T> Remove(T item);
        public Task<T> Remove(int id);
        public Task<T> Recover(T item);
        public Task<T> Recover(int id);
    }
}
