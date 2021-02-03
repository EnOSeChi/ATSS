using ATSS.Application.Common.Interfaces;
using ATSS.Application.Flights.Commands.CreateFlight;
using ATSS.Domain.Entities;
using ATSS.Domain.ValueObjects;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ATSS.Application.UnitTests.Flights.Commands.CreateFlight
{
    /// <summary>
    /// Możliwość ręcznego dodania lotu
    /// </summary>
    public class CreateFlightCommandTests : Testing
    {
        [Test]
        public async Task ShouldPersistNewFlightInDbContext()
        {
            // arrange
            Context.Tenants.Add(new Tenant
            {
                Id = 1,
                Name = "test",
                TenantGroup = Domain.Enums.TenantGroup.A
            });
            Context.SaveChanges();

            //act
            var requesHandler = new CreateFlightCommandHandler(Context);
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

            //assert
            Assert.IsTrue(Context.Flights.Count() == 1);
        }
    }
}
