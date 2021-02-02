using ATSS.Domain.Common;
using ATSS.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ATSS.Domain.ValueObjects
{
    public class FlightId : ValueObject
    {
        static FlightId()
        {
        }

        private FlightId()
        {
        }

        private FlightId(string id)
        {
            var match = Regex.Match(id, @"^[a-zA-Z]{3}\s[0-9]{5}\s[a-zA-Z]{3}$");

            if (!match.Success)
            {
                throw new UnsupportedFlightIdException(id);
            }

            Segment1 = id.Substring(0, 3);
            Segment2 = id.Substring(4, 5);
            Segment3 = id.Substring(10, 3);
        }

        public static FlightId From(string id)
        {
            var flightId = new FlightId(id);
            return flightId;
        }

        public string Segment1 { get; private set; }
        public string Segment2 { get; private set; }
        public string Segment3 { get; private set; }

        public static implicit operator string(FlightId id)
        {
            return id.ToString();
        }

        public static explicit operator FlightId(string id)
        {
            return From(id);
        }

        public override string ToString()
        {
            return $"{Segment1} {Segment2} {Segment3}";
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            return new[] { Segment1, Segment2, Segment3 };
        }
    }
}
