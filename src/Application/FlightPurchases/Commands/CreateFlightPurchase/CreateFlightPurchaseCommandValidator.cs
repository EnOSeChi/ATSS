using ATSS.Application.Common.Interfaces;
using FluentValidation;
using System;

namespace ATSS.Application.FlightPurchases.Commands.CreateFlightPurchase
{
    public class CreateFlightPurchaseCommandValidator : AbstractValidator<CreateFlightPurchaseCommand>
    {
        private readonly IApplicationDbContext _context;

        public CreateFlightPurchaseCommandValidator(IApplicationDbContext context)
        {
            _context = context;

            // prawdopodobnie dobrze sprawdzić czy ktoś nie próbuje kupić lotu z przeszłą datą
            RuleFor(v => v.Flight.DepartureDateTime)
                .GreaterThan(DateTimeOffset.UtcNow).WithMessage("Cannot purchase flight from the past.");
        }
    }
}
