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
    /// <summary>
    /// wyszukanie lotu po FlightId
    /// </summary>
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
                var datesInRange = await GetWeekdayInRange(request.From, request.To, DayOfWeek.Monday);
                result.AddRange(await GetFlightsForDates(flight, datesInRange));
            }

            if (flight.Tuesday)
            {
                var datesInRange = await GetWeekdayInRange(request.From, request.To, DayOfWeek.Tuesday);
                result.AddRange(await GetFlightsForDates(flight, datesInRange));
            }

            if (flight.Wednesday)
            {
                var datesInRange = await GetWeekdayInRange(request.From, request.To, DayOfWeek.Wednesday);
                result.AddRange(await GetFlightsForDates(flight, datesInRange));
            }

            if (flight.Thursday)
            {
                var datesInRange = await GetWeekdayInRange(request.From, request.To, DayOfWeek.Thursday);
                result.AddRange(await GetFlightsForDates(flight, datesInRange));
            }

            if (flight.Friday)
            {
                var datesInRange = await GetWeekdayInRange(request.From, request.To, DayOfWeek.Friday);
                result.AddRange(await GetFlightsForDates(flight, datesInRange));
            }

            if (flight.Saturday)
            {
                var datesInRange = await GetWeekdayInRange(request.From, request.To, DayOfWeek.Saturday);
                result.AddRange(await GetFlightsForDates(flight, datesInRange));
            }

            if (flight.Sunday)
            {
                var datesInRange = await GetWeekdayInRange(request.From, request.To, DayOfWeek.Sunday);
                result.AddRange(await GetFlightsForDates(flight, datesInRange));
            }

            return result;
        }

        private async Task<List<FlightDto>> GetFlightsForDates(Domain.Entities.Flight flight, List<DateTimeOffset> daysInRange)
        {
            var flights = new List<FlightDto>();

            foreach (var date in daysInRange)
            {
                var flightDto = _mapper.Map<FlightDto>(flight);
                flightDto.DepartureDateTime = new DateTimeOffset(date.Year, date.Month, date.Day, flight.Hour, flight.Minute, 0, TimeSpan.Zero);
                flightDto.Price = await GetPriceForDate(flight, flightDto.DepartureDateTime);
                flights.Add(flightDto);
            }

            return flights;
        }

        private Task<double> GetPriceForDate(Domain.Entities.Flight flight, DateTimeOffset departureDate)
        {
            if (flight.CustomPrices.Any(x => x.From <= departureDate &&
                x.To >= departureDate))
            {
                return Task.FromResult(flight.CustomPrices
                    .Where(x => x.From <= departureDate && x.To >= departureDate)
                    .First()
                    .Value);
            }

            return Task.FromResult(flight.DefaultPrice);
        }

        public Task<List<DateTimeOffset>> GetWeekdayInRange(DateTimeOffset from, DateTimeOffset to, DayOfWeek day)
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

            return Task.FromResult(result);
        }
    }
}
