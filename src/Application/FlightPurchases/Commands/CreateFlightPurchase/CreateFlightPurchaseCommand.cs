using ATSS.Application.Common.Interfaces;
using ATSS.Application.Flights.Queries.GetAvailableFlights;
using ATSS.Domain.Entities;
using ATSS.Domain.ValueObjects;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ATSS.Application.FlightPurchases.Commands.CreateFlightPurchase
{
    /// <summary>
    /// zakupienie biletu na lot
    /// </summary>
    public class CreateFlightPurchaseCommand : IRequest<FlightPurchaseDto>
    {
        public int TenantId { get; set; }
        public FlightDto Flight { get; set; }
    }

    public class CreateFlightPurchaseCommandHandler : IRequestHandler<CreateFlightPurchaseCommand, FlightPurchaseDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateFlightPurchaseCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<FlightPurchaseDto> Handle(CreateFlightPurchaseCommand request, CancellationToken cancellationToken)
        {
            var flightPurchase = new FlightPurchase();
            flightPurchase.DepartureDateTime = request.Flight.DepartureDateTime;
            flightPurchase.Price = request.Flight.Price;
            flightPurchase.Tenant = _context.Tenants.First(x => x.Id == request.TenantId);
            flightPurchase.Flight = _context.Flights.First(x => x.FlightId.Segment1 == request.Flight.FlightId.Segment1 &&
                x.FlightId.Segment2 == request.Flight.FlightId.Segment2 &&
                x.FlightId.Segment3 == request.Flight.FlightId.Segment3);

            _context.FlightPurchases.Add(flightPurchase);
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<FlightPurchaseDto>(flightPurchase);
        }
    }
}
