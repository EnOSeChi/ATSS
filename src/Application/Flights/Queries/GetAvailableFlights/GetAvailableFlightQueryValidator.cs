using ATSS.Application.Common.Interfaces;
using ATSS.Domain.ValueObjects;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ATSS.Application.Flights.Queries.GetAvailableFlights
{
    class GetAvailableFlightQueryValidator : AbstractValidator<GetAvailableFlightQuery>
    {
        private readonly IApplicationDbContext _context;

        public GetAvailableFlightQueryValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(v => v.FlightId)
                .NotEmpty().WithMessage("FlightId is required.")
                .MustAsync(ExistedFlightId).WithMessage("Flight with such id not exists.");

            RuleFor(v => v.TenantId)
                .NotEqual(0).WithMessage("TenantId is required")
                .MustAsync(ExistedTenant).WithMessage("Tenant not exists.");
        }

        public async Task<bool> ExistedFlightId(FlightId flightId, CancellationToken cancellationToken)
        {
            return await _context.Flights
                .AnyAsync(l => l.FlightId.Segment1 == flightId.Segment1 &&
                    l.FlightId.Segment2 == flightId.Segment2 &&
                    l.FlightId.Segment3 == flightId.Segment3);
        }

        public async Task<bool> ExistedTenant(int id, CancellationToken cancellationToken)
        {
            return await _context.Tenants
                .AnyAsync(l => l.Id == id);
        }
    }
}
