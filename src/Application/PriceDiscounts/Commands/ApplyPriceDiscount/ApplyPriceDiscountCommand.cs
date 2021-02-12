using ATSS.Application.Common.Interfaces;
using ATSS.Application.FlightPurchases.Commands.CreateFlightPurchase;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;

namespace ATSS.Application.PriceDiscounts.Commands.ApplyPriceDiscount
{
    /// <summary>
    /// zastosowanie zniżki do ceny
    /// </summary>
    public class ApplyPriceDiscountCommand : IRequest<double>
    {
        public FlightPurchaseDto FlightPurchase { get; set; }
    }

    public class ApplyPriceDiscountCommandHandler : IRequestHandler<ApplyPriceDiscountCommand, double>
    {
        private readonly IApplicationDbContext _context;

        public ApplyPriceDiscountCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<double> Handle(ApplyPriceDiscountCommand request, CancellationToken cancellationToken)
        {
            var discounts = _context.PriceDiscounts
                .Include(x => x.Rules)
                .ToList();

            var purchase = _context.FlightPurchases
                .Find(request.FlightPurchase.Id);

            foreach (var discount in discounts)
            {
                await CheckConditionsAndApply(discount, request.FlightPurchase, purchase);
            }

            await _context.SaveChangesAsync(cancellationToken);
            return purchase.Price;
        }

        private async Task CheckConditionsAndApply(Domain.Entities.PriceDiscount discount, FlightPurchaseDto purchaseDto, Domain.Entities.FlightPurchase purchase)
        {
            // minimalna cena 20
            if (purchase.Price - 5 < 20)
            {
                return;
            }

            if (await ConditionMet(purchaseDto, discount))
            {
                purchase.Price -= 5;

                // zapis zniżek dla danej grupy
                if (purchaseDto.TenantGroup == Domain.Enums.TenantGroup.A)
                {
                    purchase.Discounts.Add(discount);
                }
            }
        }

        private async Task<bool> ConditionMet(FlightPurchaseDto flightPurchase, Domain.Entities.PriceDiscount discount)
        {
            var purchases = new List<FlightPurchaseDto>();
            purchases.Add(flightPurchase);
            var query = purchases.AsQueryable();

            var queryString = string.Empty;
            queryString = await BuildQueryString(discount, queryString);

            if (string.IsNullOrEmpty(queryString) || query.Where(queryString).Any())
            {
                return true;
            }

            return false;
        }

        private Task<string> BuildQueryString(Domain.Entities.PriceDiscount discount, string queryString)
        {
            foreach (var r in discount.Rules)
            {
                if (discount.Rules.IndexOf(r) == 0)
                {
                    queryString += "(";
                }

                queryString += $"{r.FieldName} = \"{r.Value}\"";

                if (discount.Rules.IndexOf(r) == discount.Rules.Count - 1)
                {
                    queryString += ")";
                    break;
                }

                queryString += " and ";
            }

            return Task.FromResult(queryString);
        }
    }
}
