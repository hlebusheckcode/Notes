using Microsoft.Extensions.DependencyInjection;
using Notes.Repository;
using Notes.SqliteRepository;
using System;
using System.Windows;

namespace Notes
{
    public partial class App : Application
    {
        public static ServiceProvider ServiceProvider = null!;

        public App()
        {
            ServiceCollection services = new ServiceCollection();
            ConfigureServices(services);
            ServiceProvider = services.BuildServiceProvider();
        }

        private void ConfigureServices(ServiceCollection services)
        {
            services.AddDbContext<DataContext>();
            services.AddScoped<IMemoRepository, MemoRepository>();
            services.AddSingleton<MainWindow>();
        }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            var mainWindow = ServiceProvider.GetService<MainWindow>();
            if (mainWindow == null)
                throw new Exception("Everything is broken.");
            mainWindow.Show();
        }
    }
}
