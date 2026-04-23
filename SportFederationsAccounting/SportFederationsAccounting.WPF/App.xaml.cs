using SportFederationsAccounting.Data;
using System.Configuration;
using System.Data;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace SportFederationsAccounting.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly string DbFileName = "SportFederations.db";
        private static readonly string LogFilePath = "error.log";

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            try
            {
                using var context = new AppDbContextFactory().CreateDbContext(null!);

                // Автоматически применяем миграции (структура обновляется, данные сохраняются)
                context.Database.Migrate();

                LogToFile("База данных успешно обновлена.");
            }
            catch (Exception ex)
            {
                string errorMessage = $"Ошибка при инициализации базы данных:\n{ex.Message}";

                MessageBox.Show(errorMessage, "Ошибка базы данных",
                               MessageBoxButton.OK, MessageBoxImage.Error);

                LogToFile($"КРИТИЧЕСКАЯ ОШИБКА:\n{errorMessage}\n\n{ex}");
            }
        }

        /// <summary>
        /// Простое логирование ошибок в файл error.log
        /// </summary>
        private static void LogToFile(string message)
        {
            try
            {
                string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}\n\n";
                File.AppendAllText(LogFilePath, logEntry);
            }
            catch { }
        }
    }
}


