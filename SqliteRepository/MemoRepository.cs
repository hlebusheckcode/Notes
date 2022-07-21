using Microsoft.EntityFrameworkCore;
using Model;
using Repository;
using SqliteRepository.Base;

namespace SqliteRepository
{
    public class MemoRepository : RemovableRepository<Memo>, IMemoRepository
    {
        public MemoRepository(DataContext dataContext)
            : base(dataContext) { }

        public override async Task<IEnumerable<Memo>> Get()
        {
            return await _dataContext.Memos
                .Where(m => m.RemovedDate == null)
                .ToArrayAsync();
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

        public override async Task<Memo> Remove(int id)
            => await Remove(await Get(id));
        public override async Task<Memo> Remove(Memo item)
        {
            item.RemovedDate = DateTime.Now;
            _dataContext.Memos.Update(item);
            await _dataContext.SaveChangesAsync();
            return item;
        }

        public override async Task<Memo> Recover(int id)
            => await Recover(await Get(id));
        public override async Task<Memo> Recover(Memo item)
        {
            item.RemovedDate = null;
            _dataContext.Memos.Update(item);
            await _dataContext.SaveChangesAsync();
            return item;
        }
    }
}
