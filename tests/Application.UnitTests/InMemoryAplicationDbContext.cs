using ATSS.Application.Common.Interfaces;
using ATSS.Domain.Entities;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace ATSS.Application.UnitTests
{
    /// <summary>
    /// tylko do testów
    /// </summary>
    public class InMemoryAplicationDbContext : DbContext, IApplicationDbContext
    {
        public DbSet<Flight> Flights { get; set; }
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<FlightPrice> FlightPrices { get; set; }
        public DbSet<FlightPurchase> FlightPurchases { get; set; }
        public DbSet<PriceDiscount> PriceDiscounts { get; set; }
        public DbSet<PriceDiscountRule> PriceDiscountRules { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseInMemoryDatabase("test");//.UseSqlite(connection: CreateInMemoryDatabase());
            }
            base.OnConfiguring(optionsBuilder);
        }

        private DbConnection CreateInMemoryDatabase()
        {
            var connection = new SqliteConnection("Filename=:memory:");
            //var connection = new SqliteConnection("Filename=test.db");

            connection.Open();

            return connection;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Flight>(e =>
            {
                e.OwnsOne(x => x.FlightId);
            });
            base.OnModelCreating(builder);
        }
    }
}
