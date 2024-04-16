using Microsoft.EntityFrameworkCore;
using Notes.Model;
using Notes.Repository;

namespace Notes.SqliteRepository
{
    public class MemoRepository(DataContext dataContext) : IMemoRepository
    {
        private readonly DataContext _dataContext = dataContext;

        public async Task<IEnumerable<Memo>> Get() =>
            await _dataContext.Memos.ToArrayAsync();

        public async Task<Memo> Get(uint id) =>
            await _dataContext.Memos
                .Where(m => m.Id == id)
                .FirstAsync();

        public async Task<Memo> Insert(Memo item)
        {
            item.InsertedDate = DateTime.Now;
            item.UpdatedDate = DateTime.Now;
            var entry = await _dataContext.Memos.AddAsync(item);
            _ = await _dataContext.SaveChangesAsync();
            return entry.Entity;
        }

        public async Task<Memo> Update(Memo item)
        {
            if (item.HasChanges)
                item.UpdatedDate = DateTime.Now;
            var entry = _dataContext.Memos.Update(item);
            _ = await _dataContext.SaveChangesAsync();
            return entry.Entity;
        }

        public async Task<Memo> Remove(Memo item)
        {
            item.UpdatedDate = DateTime.Now;
            item.RemovedDate = DateTime.Now;
            var entry = _dataContext.Memos.Update(item);
            _ = await _dataContext.SaveChangesAsync();
            return entry.Entity;
        }
        public async Task<Memo> Recover(Memo item)
        {
            item.UpdatedDate = DateTime.Now;
            item.RemovedDate = null;
            var entry = _dataContext.Memos.Update(item);
            _ = await _dataContext.SaveChangesAsync();
            return entry.Entity;
        }

        public async Task<Memo> Delete(Memo item)
        {
            var entry = _dataContext.Memos.Remove(item);
            _ = await _dataContext.SaveChangesAsync();
            return entry.Entity;
        }

        public async Task Import(IEnumerable<Memo> items)
        {
            await _dataContext.Memos.AddRangeAsync(items);
            _ = await _dataContext.SaveChangesAsync();
        }
    }
}
