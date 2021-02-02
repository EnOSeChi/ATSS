using ATSS.Application.Common.Interfaces;
using ATSS.Application.Flights.Commands.CreateFlight;
using ATSS.Domain.Entities;
using ATSS.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ATSS.Application.UnitTests.Flights.Commands.CreateFlight
{
    /// <summary>
    /// Możliwość ręcznego dodania lotu
    /// </summary>
    public class CreateFlightCommandTests
    {
        public CreateFlightCommandTests()
        {

        }

        [Test]
        public async Task ShouldPersistNewFlightInDbContext()
        {
            using (var context = new InMemoryAplicationDbContext())
            {
                context.Database.EnsureCreated();

                // arrange
                context.Tenants.Add(new Tenant
                {
                    Id = 1,
                    Name = "test",
                    TenantGroup = Domain.Enums.TenantGroup.A
                });
                context.SaveChanges();

                var requesHandler = new CreateFlightCommandHandler(context);
                await requesHandler.Handle(new CreateFlightCommand
                {
                    FlightId = FlightId.From("KLM 12345 BCA"),
                    Days = new List<DayOfWeek> { DayOfWeek.Monday },
                    DefaultPrice = 24,
                    From = "Europe",
                    To = "Africa",
                    Hour = 10,
                    Minute = 30,
                    TenantId = 1
                }, CancellationToken.None);
                Assert.IsTrue(context.Flights.Count() == 1);

                context.Database.EnsureDeleted();
            }
        }
    }
}
