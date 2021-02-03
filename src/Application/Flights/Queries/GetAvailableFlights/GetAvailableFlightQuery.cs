using ATSS.Application.Common.Interfaces;
using ATSS.Domain.ValueObjects;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ATSS.Application.Flights.Queries.GetAvailableFlights
{
    public class GetAvailableFlightQuery : IRequest<List<FlightDto>>
    {
        public FlightId FlightId { get; set; }
        public DateTimeOffset From { get; set; }
        public DateTimeOffset To { get; set; }
        public int TenantId { get; set; }
    }

    public class GetAvailableFlightQueryHandler : IRequestHandler<GetAvailableFlightQuery, List<FlightDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetAvailableFlightQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<FlightDto>> Handle(GetAvailableFlightQuery request, CancellationToken cancellationToken)
        {
            var flight = await _context.Flights
                .Include(x => x.CustomPrices)
                .Where(x => x.FlightId.Segment1 == request.FlightId.Segment1 &&
                    x.FlightId.Segment2 == request.FlightId.Segment2 &&
                    x.FlightId.Segment3 == request.FlightId.Segment3)
                .FirstAsync();

            var result = new List<FlightDto>();

            if (flight.Monday)
            {
                var mondaysInRange = GetWeekdayInRange(request.From, request.To, DayOfWeek.Monday);

                foreach (var date in mondaysInRange)
                {
                    var flightDto = _mapper.Map<FlightDto>(flight);
                    flightDto.DepartureDateTime = new DateTimeOffset(date.Year, date.Month, date.Day, flight.Hour, flight.Minute, 0, TimeSpan.Zero);
                    flightDto.Price = GetPriceForDate(flight, flightDto.DepartureDateTime);
                    result.Add(flightDto);
                }
            }

            return result;
        }

        private double GetPriceForDate(Domain.Entities.Flight flight, DateTimeOffset departureDate)
        {
            if (flight.CustomPrices.Any(x => x.From <= departureDate &&
                x.To >= departureDate))
            {
                return flight.CustomPrices
                    .Where(x => x.From <= departureDate && x.To >= departureDate)
                    .First()
                    .Value;
            }
            else
            {
                return flight.DefaultPrice;
            }
        }

        public List<DateTimeOffset> GetWeekdayInRange(DateTimeOffset from, DateTimeOffset to, DayOfWeek day)
        {
            const int daysInWeek = 7;
            var result = new List<DateTimeOffset>();
            var daysToAdd = ((int)day - (int)from.DayOfWeek + daysInWeek) % daysInWeek;
            
            while (from.AddDays(daysToAdd) <= to)
            { 
                from = from.AddDays(daysToAdd);
                result.Add(from);
                daysToAdd = daysInWeek;
            }

            return result;
        }
    }
}
