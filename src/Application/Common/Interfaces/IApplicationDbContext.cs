using ATSS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace ATSS.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Flight> Flights { get; set; }

        DbSet<Tenant> Tenants { get; set; }

        DbSet<FlightPrice> FlightPrices { get; set; }

        DbSet<FlightPurchase> FlightPurchases { get; set; }

        DbSet<PriceDiscount> PriceDiscounts { get; set; }

        DbSet<PriceDiscountRule> PriceDiscountRules { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
