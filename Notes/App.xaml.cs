using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Repository;
using SqliteRepository;
using System;
using System.IO;
using System.Windows;

namespace Notes
{
    public partial class App : Application
    {
        private ServiceProvider serviceProvider;

        public App()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
#if DEBUG
            var dbName = "notes.test.db";
#else
            var dbName = "notes.db";
#endif
            DbPath = Path.Join(path, dbName);
            ServiceCollection services = new ServiceCollection();
            ConfigureServices(services);
            serviceProvider = services.BuildServiceProvider();
        }

        public string DbPath { get; set; }

        private void ConfigureServices(ServiceCollection services)
        {
            services.AddDbContext<DataContext>(options => options.UseSqlite($"Data Source={DbPath}"));
            services.AddScoped<IMemoRepository, MemoRepository>();
            services.AddSingleton<MainWindow>();
        }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            var mainWindow = serviceProvider.GetService<MainWindow>();
            mainWindow?.Show();
        }
    }
}
