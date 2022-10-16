using Microsoft.EntityFrameworkCore;
using Model;
using Model.Base;

namespace SqliteRepository
{
    public class DataContext : DbContext
    {
        public DataContext()
        {
            ConfigureDbPath();
        }
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
            ConfigureDbPath();
        }

        public string DbPath { get; set; }

        public DbSet<Memo> Memos => Set<Memo>();

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var insertedEntries = ChangeTracker.Entries()
                               .Where(x => x.State == EntityState.Added)
                               .Select(x => x.Entity);

            foreach (var insertedEntry in insertedEntries)
            {
                var auditableEntity = insertedEntry as IAuditableEntity;
                if (auditableEntity != null)
                {
                    auditableEntity.InsertedDate = DateTime.Now;
                }
            }

            var modifiedEntries = ChangeTracker.Entries()
                       .Where(x => x.State == EntityState.Modified)
                       .Select(x => x.Entity);

            foreach (var modifiedEntry in modifiedEntries)
            {
                var auditableEntity = modifiedEntry as IAuditableEntity;
                if (auditableEntity != null)
                {
                    auditableEntity.UpdatedDate = DateTime.Now;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlite($"Data Source={DbPath}");
        }

        private void ConfigureDbPath()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
#if DEBUG
            var dbName = "notes.test.db";
#else
            var dbName = "notes.db";
#endif
            DbPath = Path.Join(path, dbName);
        }
    }
}
