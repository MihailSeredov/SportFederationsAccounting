using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using SportFederationsAccounting.Data;

namespace SportFederationsAccounting.Data
{
    /// <summary>
    /// Фабрика контекста для создания миграций и работы в design-time
    /// </summary>
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            // База данных будет лежать рядом с exe-файлом
            string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SportFederations.db");

            optionsBuilder.UseSqlite($"Data Source={dbPath}");

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}