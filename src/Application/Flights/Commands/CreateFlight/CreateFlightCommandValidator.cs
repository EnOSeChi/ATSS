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

namespace ATSS.Application.Flights.Commands.CreateFlight
{
    public class CreateFlightCommandValidator : AbstractValidator<CreateFlightCommand>
    {
        private readonly IApplicationDbContext _context;

        public CreateFlightCommandValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(v => v.FlightId)
                .NotEmpty().WithMessage("FlightId is required.")
                .MustAsync(UniqueFlightId).WithMessage("The specified flight id already exists.");

            RuleFor(v => v.TenantId)
                .NotEqual(0).WithMessage("TenantId is required")
                .MustAsync(ExistedTenant).WithMessage("Tenant not exists.");
        }

        public async Task<bool> UniqueFlightId(FlightId flightId, CancellationToken cancellationToken)
        {
            return await _context.Flights
                .AllAsync(l => l.FlightId != flightId);
        }

        public async Task<bool> ExistedTenant(int id, CancellationToken cancellationToken)
        {
            return await _context.Tenants
                .AnyAsync(l => l.Id == id);
        }
    }
}
