using FluentAssertions;
using NUnit.Framework;
using RecruitmentTask.Domain.Exceptions;
using RecruitmentTask.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecruitmentTask.Domain.UnitTests.ValueObjects
{
    public class FlightIdTests
    {
        [Test]
        public void ShouldReturnCorrectFlightIdSegments()
        {
            var id = "KLM 12345 BCA";
            var flightId = FlightId.From(id);
            flightId.Segment1.Should().Be("KLM");
            flightId.Segment2.Should().Be("12345");
            flightId.Segment3.Should().Be("BCA");
        }

        [Test]
        public void ToStringReturnsId()
        {
            var id = "KLM 12345 BCA";
            var flightId = FlightId.From(id);
            flightId.ToString().Should().Be(id);
        }

        [Test]
        public void ShouldThrowUnsupportedFlightIdExceptionGivenNotSupportedId()
        {
            FluentActions.Invoking(() => FlightId.From("KLM 12345 BCAx"))
                .Should().Throw<UnsupportedFlightIdException>();
        }
    }
}
