using ATSS.Application.Common.Mappings;
using ATSS.Application.FlightPurchases.Commands.CreateFlightPurchase;
using ATSS.Application.Flights.Queries.GetAvailableFlights;
using ATSS.Domain.Entities;
using ATSS.Domain.ValueObjects;
using AutoMapper;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ATSS.Application.UnitTests.FlightPurchases.Commands.CreateFlightPurchase
{
    /// <summary>
    /// kupienie lotu
    /// </summary>
    public class CreateFlightPurchaseCommandTests : Testing
    {
        private MapperConfiguration _configuration;
        private IMapper _mapper;

        public CreateFlightPurchaseCommandTests()
        {
            _configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            _mapper = _configuration.CreateMapper();
        }

        [Test]
        public async Task ShouldPersistNewFlightPurchaseInDbContext()
        {
            var tenant = new Tenant
            {
                Id = 1,
                Name = "test",
                TenantGroup = Domain.Enums.TenantGroup.A
            };
            Context.Tenants.Add(tenant);
            Context.Flights.Add(new Flight
            {
                FlightId = FlightId.From("KLM 12345 BCA"),
                Monday = true,
                DefaultPrice = 24,
                From = "Europe",
                To = "Africa",
                Hour = 10,
                Minute = 30,
                CreatedBy = tenant
            });

            Context.SaveChanges();

            var from = DateTimeOffset.Parse("2021-02-02T10:00:00Z");

            var availableFlightsQuery = new GetAvailableFlightQueryHandler(Context, _mapper);
            var result = await availableFlightsQuery.Handle(new GetAvailableFlightQuery
            {
                FlightId = FlightId.From("KLM 12345 BCA"),
                From = from,
                To = from.AddDays(7)
            }, CancellationToken.None);

            var createFlightPurchaseCommand = new CreateFlightPurchaseCommandHandler(Context, _mapper);
            var purchase = await createFlightPurchaseCommand.Handle(new CreateFlightPurchaseCommand
            {
                Flight = result.First(),
                TenantId = 1
            }, CancellationToken.None);

            //assert
            Assert.IsTrue(Context.FlightPurchases.Count() == 1);
        }
    }
}
