using Notes.Model;

namespace Notes.Repository
{
    public interface IMemoRepository
    {
        public Task<IEnumerable<Memo>> Get();
        public Task<Memo> Get(uint id);

        public Task<Memo> Insert(Memo item);

        public Task<Memo> Update(Memo item);

        public Task<Memo> Remove(Memo item);
        public Task<Memo> Recover(Memo item);

        public Task<Memo> Delete(Memo item);

        public Task Import(IEnumerable<Memo> items);
    }
}
