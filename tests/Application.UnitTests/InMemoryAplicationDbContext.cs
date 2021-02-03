using ATSS.Application.Common.Interfaces;
using ATSS.Domain.Entities;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ATSS.Application.UnitTests
{
    /// <summary>
    /// for tests purposes only
    /// </summary>
    public class InMemoryAplicationDbContext : DbContext, IApplicationDbContext
    {
        public DbSet<TodoList> TodoLists { get; set; }
        public DbSet<TodoItem> TodoItems { get; set; }
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
                optionsBuilder.UseSqlite(connection: CreateInMemoryDatabase());
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
            builder.Entity<TodoItem>(e => e.Ignore(x => x.DomainEvents));
            builder.Entity<TodoList>(e => e.Ignore(x => x.Colour));
            builder.Entity<Flight>(e =>
            {
                e.OwnsOne(x => x.FlightId);
            });
            base.OnModelCreating(builder);
        }
    }
}
