using ATSS.Application.Common.Mappings;
using ATSS.Application.Flights.Commands.CreateFlight;
using ATSS.Application.Flights.Queries.GetAvailableFlights;
using ATSS.Domain.Entities;
using ATSS.Domain.ValueObjects;
using AutoMapper;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ATSS.Application.UnitTests.Flights.Queries.GetAvailableFlights
{
    public class GetAvailableFlightsQueryTests : Testing
    {
        private MapperConfiguration _configuration;
        private IMapper _mapper;

        public GetAvailableFlightsQueryTests()
        {
            _configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            _mapper = _configuration.CreateMapper();
        }

        [Test]
        public async Task ShouldReturnOneFlightWithDefaultPrice()
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

            Assert.True(result.Count == 1);
            Assert.AreEqual("2021-02-08 10:30:00", result.First().DepartureDateTime.ToString("yyyy-MM-dd HH:mm:ss"));
            Assert.AreEqual(24, result.First().Price);
        }

        [Test]
        public async Task ShouldReturnZeroResults()
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
                To = from.AddDays(4)
            }, CancellationToken.None);

            Assert.True(result.Count == 0);
        }

        [Test]
        public async Task ShouldReturnTwoFlightsWithDefaultPrice()
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
                To = from.AddDays(13)
            }, CancellationToken.None);

            Assert.True(result.Count == 2);
            var firstResult = result.First();
            var secondResult = result.Skip(1).First();
            Assert.AreEqual("2021-02-08 10:30:00", firstResult.DepartureDateTime.ToString("yyyy-MM-dd HH:mm:ss"));
            Assert.AreEqual(24, firstResult.Price);
            Assert.AreEqual("2021-02-15 10:30:00", secondResult.DepartureDateTime.ToString("yyyy-MM-dd HH:mm:ss"));
            Assert.AreEqual(24, secondResult.Price);
        }

        [Test]
        public async Task ShouldReturnOneFlightWithCustomPrice()
        {
            var from = DateTimeOffset.Parse("2021-02-02T10:00:00Z");
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
                CreatedBy = tenant,
                CustomPrices = new List<FlightPrice>
                {
                    new FlightPrice
                    {
                        From = from,
                        To = from.AddDays(10),
                        Value = 30
                    }
                }
            });

            Context.SaveChanges();

            var availableFlightsQuery = new GetAvailableFlightQueryHandler(Context, _mapper);
            var result = await availableFlightsQuery.Handle(new GetAvailableFlightQuery
            {
                FlightId = FlightId.From("KLM 12345 BCA"),
                From = from,
                To = from.AddDays(7)
            }, CancellationToken.None);

            Assert.True(result.Count == 1);
            Assert.AreEqual("2021-02-08 10:30:00", result.First().DepartureDateTime.ToString("yyyy-MM-dd HH:mm:ss"));
            Assert.AreEqual(30, result.First().Price);
        }
    }
}
