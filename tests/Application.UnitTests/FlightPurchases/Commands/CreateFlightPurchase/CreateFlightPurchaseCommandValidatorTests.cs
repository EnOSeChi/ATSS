using ATSS.Application.Common.Exceptions;
using ATSS.Application.FlightPurchases.Commands.CreateFlightPurchase;
using ATSS.Application.Flights.Queries.GetAvailableFlights;
using ATSS.Domain.Exceptions;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ATSS.Application.UnitTests.FlightPurchases.Commands.CreateFlightPurchase
{
    public class CreateFlightPurchaseCommandValidatorTests : Testing
    {
        [Test]
        public async Task ShouldThrowValidationExceptionForPastDate()
        {
            var flight = new FlightDto
            {
                DepartureDateTime = DateTimeOffset.UtcNow.AddDays(-10)
            };

            var validator = new CreateFlightPurchaseCommandValidator(Context);

            var result = await validator.ValidateAsync(new CreateFlightPurchaseCommand
            {
                Flight = flight,
                TenantId = 1
            }, CancellationToken.None);

            Assert.True(result.Errors.Any());
            Assert.True(result.Errors.Any(x => x.ErrorMessage == "Cannot purchase flight from the past."));
        }
    }
}
