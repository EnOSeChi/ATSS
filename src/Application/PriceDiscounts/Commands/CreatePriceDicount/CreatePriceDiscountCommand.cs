using ATSS.Application.Common.Interfaces;
using ATSS.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ATSS.Application.PriceDiscounts.Commands.CreatePriceDiscount
{
    public class CreatePriceDiscountCommand : IRequest<int>
    {
        public CreatePriceDiscountCommand()
        {
            Rules = new List<PriceDiscountRule>();
        }

        public IList<PriceDiscountRule> Rules { get; private set; }
    }

    public class CreatePriceDiscountCommandHandler : IRequestHandler<CreatePriceDiscountCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public CreatePriceDiscountCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreatePriceDiscountCommand request, CancellationToken cancellationToken)
        {
            var priceDiscount = new PriceDiscount();
            request.Rules.ToList().ForEach(x =>
            {
                priceDiscount.Rules.Add(x);
            });

            _context.PriceDiscounts.Add(priceDiscount);
            await _context.SaveChangesAsync(cancellationToken);
            return priceDiscount.Id;
        }
    }
}
