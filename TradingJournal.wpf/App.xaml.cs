using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using TradingJournal.core.Data;
using TradingJournal.core.Interfaces;
using TradingJournal.core.Data;
using TradingJournal.core.Interfaces;
using TradingJournal.wpf.ViewModels;
using TradingJournal.wpf.ViewModels;

namespace TradingJournal.Wpf
{
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; } = null!;

        protected override void OnStartup(StartupEventArgs e)
        {
            var services = new ServiceCollection();

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(
                    @"Server=localhost\SQLEXPRESS01;Database=TradingJournalDb;Trusted_Connection=True;TrustServerCertificate=True;"));

            services.AddScoped<ITradeRepository, TradeRepository>();
            services.AddTransient<MainViewModel>();

            ServiceProvider = services.BuildServiceProvider();

            using var scope = ServiceProvider.CreateScope();
            var context = scope.ServiceProvider
                .GetRequiredService<AppDbContext>();
            context.Database.EnsureCreated();

            var mainWindow = new MainWindow();
            mainWindow.DataContext = new MainViewModel();
            mainWindow.Show();

            base.OnStartup(e);
        }
    }
}