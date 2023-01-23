using Baza.Repository;
using Microsoft.EntityFrameworkCore;
using Notes.Model;
using Notes.Repository;

namespace Notes.SqliteRepository
{
    public class MemoRepository : Repository<Memo>, IMemoRepository
    {
        protected DataContext _dataContext;

        public MemoRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public override async Task<IEnumerable<Memo>> Get()
        {
            return await Get(RemoveOption.All);
        }
        public async Task<IEnumerable<Memo>> Get(RemoveOption removeOption)
        {
            var items = await _dataContext.Memos.ToArrayAsync();

            return items.Where(m => removeOption switch
            {
                RemoveOption.All => true,
                RemoveOption.OnlyRemoved => m.Removed == true,
                _ => m.Removed == false
            });
        }
        public override async Task<Memo> Get(int id)
        {
            return await _dataContext.Memos
                .Where(m => m.Id == id)
                .FirstAsync();
        }

        public override async Task<Memo> Insert(Memo item)
        {
            await _dataContext.Memos.AddAsync(item);
            await _dataContext.SaveChangesAsync();
            return item;
        }

        public override async Task<Memo> Update(int id, Memo item)
        {
            item.Id = id;
            return await Update(item);
        }
        public override async Task<Memo> Update(Memo item)
        {
            _dataContext.Memos.Update(item);
            await _dataContext.SaveChangesAsync();
            return item;
        }

        public override async Task<Memo> Delete(int id)
            => await Delete(await Get(id));
        public override async Task<Memo> Delete(Memo item)
        {
            _dataContext.Remove(item);
            await _dataContext.SaveChangesAsync();
            return item;
        }

        public async Task<Memo> Remove(int id)
            => await Remove(await Get(id));
        public async Task<Memo> Remove(Memo item)
        {
            item.RemovedDate = DateTime.Now;
            _dataContext.Memos.Update(item);
            await _dataContext.SaveChangesAsync();
            return item;
        }

        public async Task<Memo> Recover(int id)
            => await Recover(await Get(id));
        public async Task<Memo> Recover(Memo item)
        {
            item.RemovedDate = null;
            _dataContext.Memos.Update(item);
            await _dataContext.SaveChangesAsync();
            return item;
        }

        public async Task Import(IEnumerable<Memo> items)
        {
            _dataContext.AutoSetInsertedDate = _dataContext.AutoSetUpdatedDate = false;
            await _dataContext.Memos.AddRangeAsync(items);
            await _dataContext.SaveChangesAsync();
            _dataContext.AutoSetInsertedDate = _dataContext.AutoSetUpdatedDate = true;
        }
    }
}
