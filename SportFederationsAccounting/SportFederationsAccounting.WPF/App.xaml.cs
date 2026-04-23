using Microsoft.EntityFrameworkCore;
using SportFederationsAccounting.Core.Models;
using SportFederationsAccounting.Data;
using System;
using System.IO;
using System.Windows;

namespace SportFederationsAccounting.WPF
{
    public partial class App : Application
    {
        private static readonly string LogFilePath = "error.log";

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            try
            {
                using var context = new AppDbContextFactory().CreateDbContext(null!);

                context.Database.EnsureCreated();

                InitializeDictionaries(context);

                LogToFile("База данных успешно инициализирована.");
            }
            catch (Exception ex)
            {
                string innerMessage = ex.InnerException?.Message ?? "Нет внутренней ошибки";
                string fullError = $"Ошибка при инициализации базы:\n\n{innerMessage}\n\nПолная ошибка:\n{ex}";

                MessageBox.Show(fullError, "Критическая ошибка",
                               MessageBoxButton.OK, MessageBoxImage.Error);

                LogToFile($"КРИТИЧЕСКАЯ ОШИБКА:\n{fullError}");
            }
        }

        private void InitializeDictionaries(AppDbContext context)
        {
            // Вид спорта
            if (!context.SportTypes.Any())
            {
                context.SportTypes.AddRange(new[]
                {
            new SportType { Id = 1, Name = "Лыжные гонки" },
            new SportType { Id = 2, Name = "Биатлон" },
            new SportType { Id = 3, Name = "Фигурное катание" },
            new SportType { Id = 4, Name = "Хоккей" },
            new SportType { Id = 5, Name = "Футбол" }
        });
            }

            // Статус аккредитации
            if (!context.AccreditationStatuses.Any())
            {
                context.AccreditationStatuses.AddRange(new[]
                {
            new AccreditationStatus { Id = 1, Name = "Аккредитована" },
            new AccreditationStatus { Id = 2, Name = "Аккредитация в процессе" },
            new AccreditationStatus { Id = 3, Name = "Отказ в аккредитации" },
            new AccreditationStatus { Id = 4, Name = "Аккредитация завершена" }
        });
            }

            // Состояние федерации
            if (!context.FederationStates.Any())
            {
                context.FederationStates.AddRange(new[]
                {
            new FederationState { Id = 1, Name = "Действующая" },
            new FederationState { Id = 2, Name = "В процессе реорганизации" },
            new FederationState { Id = 3, Name = "В статусе ликвидации" },
            new FederationState { Id = 4, Name = "Ликвидирована" }
        });
            }

            context.SaveChanges();
        }

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