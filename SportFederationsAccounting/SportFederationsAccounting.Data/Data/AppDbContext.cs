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

            // Уникальность кодов
            modelBuilder.Entity<Federation>()
                .HasIndex(f => f.Code)
                .IsUnique();

            modelBuilder.Entity<SportType>()
                .HasIndex(s => s.Code)
                .IsUnique();

            modelBuilder.Entity<AccreditationStatus>()
                .HasIndex(s => s.Code)
                .IsUnique();

            modelBuilder.Entity<FederationState>()
                .HasIndex(s => s.Code)
                .IsUnique();

            // Связь ContactPerson с Federation
            modelBuilder.Entity<ContactPerson>()
                .HasOne(c => c.Federation)
                .WithMany(f => f.ContactPersons)
                .HasForeignKey(c => c.FederationId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}