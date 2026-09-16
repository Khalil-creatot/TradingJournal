using System.Configuration;
using System.Data;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using TradingJournal.core.Data;
using TradingJournal.core.Interfaces;   


namespace TradingJournal.wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; } = null!;

        protected override void OnStartup(StartupEventArgs e)
        {
            var service = new ServiceCollection();
            
            service.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(@"Server=Alizadeh\SQLEXPRESS01;Database=TradingJournalDb;Trusted_Connection=True;TrustServerCertificate=True;");
            });

            // Register the TradeRepository with the DI container
            service.AddScoped<ITradeRepository, TradeRepository>();

            // Build the service provider
            ServiceProvider = service.BuildServiceProvider();

            // Create a scope to resolve the AppDbContext and ensure the database is created
            using var scope = ServiceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();


            // Ensure the database is created
            base.OnStartup(e);
        }
    }

}
