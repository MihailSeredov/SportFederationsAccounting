using Microsoft.EntityFrameworkCore;
using SportFederationsAccounting.Core.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace SportFederationsAccounting.Data
{
    /// <summary>
    /// Основной контекст базы данных
    /// </summary>
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Таблицы
        public DbSet<Federation> Federations { get; set; } = null!;
        public DbSet<SportType> SportTypes { get; set; } = null!;
        public DbSet<AccreditationStatus> AccreditationStatuses { get; set; } = null!;
        public DbSet<FederationState> FederationStates { get; set; } = null!;
        public DbSet<ContactPerson> ContactPersons { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Настройка связей
            modelBuilder.Entity<Federation>()
                .HasOne(f => f.SportType)
                .WithMany()
                .HasForeignKey(f => f.SportTypeId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Federation>()
                .HasOne(f => f.AccreditationStatus)
                .WithMany()
                .HasForeignKey(f => f.AccreditationStatusId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Federation>()
                .HasOne(f => f.FederationState)
                .WithMany()
                .HasForeignKey(f => f.FederationStateId)
                .OnDelete(DeleteBehavior.SetNull);

            // Уникальность кода федерации
            modelBuilder.Entity<Federation>()
                .HasIndex(f => f.Code)
                .IsUnique();
        }
    }
}