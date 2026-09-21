using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TradingJournal.core.Data;
using TradingJournal.core.Interfaces;
using TradingJournal.wpf.ViewModels;

namespace TradingJournal.wpf
{
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; } = null!;

        protected override void OnStartup(StartupEventArgs e)
        {
            // Registrera automatisk markering av text vid klick/fokus för alla textrutor
            EventManager.RegisterClassHandler(typeof(TextBox), UIElement.GotKeyboardFocusEvent, new KeyboardFocusChangedEventHandler(SelectAllText));
            EventManager.RegisterClassHandler(typeof(TextBox), UIElement.PreviewMouseLeftButtonDownEvent, new MouseButtonEventHandler(SelectivelyIgnoreMouseButton));

            try
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
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Fel vid uppstart:\n\n{ex.Message}\n\nInner: {ex.InnerException?.Message}",
                    "Startfel",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                Shutdown();
            }
        }

        private static void SelectAllText(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                textBox.SelectAll();
            }
        }

        private static void SelectivelyIgnoreMouseButton(object sender, MouseButtonEventArgs e)
        {
            if (sender is TextBox textBox && !textBox.IsKeyboardFocusWithin)
            {
                textBox.Focus();
                e.Handled = true;
            }
        }
    }
}