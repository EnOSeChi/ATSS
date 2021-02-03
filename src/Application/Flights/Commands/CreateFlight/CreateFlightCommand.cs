using ATSS.Application.Common.Interfaces;
using ATSS.Domain.Entities;
using ATSS.Domain.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ATSS.Application.Flights.Commands.CreateFlight
{
    /// <summary>
    /// Możliwość ręcznego dodania lotu
    /// </summary>
    public class CreateFlightCommand: IRequest<int>
    {
        public FlightId FlightId { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public int Hour { get; set; }
        public int Minute { get; set; }
        public double DefaultPrice { get; set; }
        public List<DayOfWeek> Days { get; set; }
        public int TenantId { get; set; }
    }

    public class CreateFlightCommandHandler : IRequestHandler<CreateFlightCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public CreateFlightCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateFlightCommand request, CancellationToken cancellationToken)
        {
            var entity = new Flight
            {
                FlightId = request.FlightId,
                DefaultPrice = request.DefaultPrice,
                From = request.From,
                To = request.To,
                Hour = request.Hour,
                Minute = request.Minute,
                Monday = request.Days.Contains(DayOfWeek.Monday),
                Tuesday = request.Days.Contains(DayOfWeek.Tuesday),
                Wednesday = request.Days.Contains(DayOfWeek.Wednesday),
                Thursday = request.Days.Contains(DayOfWeek.Thursday),
                Friday = request.Days.Contains(DayOfWeek.Friday),
                Saturday = request.Days.Contains(DayOfWeek.Saturday),
                Sunday = request.Days.Contains(DayOfWeek.Sunday)
            };

            entity.CreatedBy = _context.Tenants.First(x => x.Id == request.TenantId);

            _context.Flights.Add(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}
