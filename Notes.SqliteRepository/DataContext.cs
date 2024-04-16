using Microsoft.EntityFrameworkCore;
using Notes.Model;

namespace Notes.SqliteRepository
{
    public class DataContext : DbContext
    {
        public DataContext()
        {
            ConfigureDefaultDbPath();
        }
        public DataContext(DbContextOptions<DataContext> options) : this(options, null) { }
        public DataContext(DbContextOptions<DataContext> options, string? path)
            : base(options)
        {
            DbPath = string.IsNullOrWhiteSpace(path)
                ? ConfigureDefaultDbPath()
                : path;

            Database.Migrate();
        }

        public string DbPath { get; set; } = string.Empty;

        public DbSet<Memo> Memos => Set<Memo>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Memo>().OwnsOne(memo => memo.Options);

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlite(@$"Data Source={DbPath}");
        }

        private string ConfigureDefaultDbPath()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
#if DEBUG
            var dbName = "notes.test.db";
#else
            var dbName = "notes.db";
#endif
            return Path.Join(path, dbName);
        }
    }
}
