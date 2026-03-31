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

            // Строка подключения к SQLite
            var connectionString = "Data Source=SportFederations.db";

            optionsBuilder.UseSqlite(connectionString);

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}