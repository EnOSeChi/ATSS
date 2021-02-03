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
    /// <summary>
    /// Flight Id – oznacza unikalne ID lotu. Składa się z trzech liter (oznaczający kod IATA linii lotniczej
    /// fizycznie wykonującej lot), 5-ciu cyfr i trzech liter na końcu.
    /// Przykład: KLM 12345 BCA
    /// </summary>
    public class FlightId : ValueObject
    {
        static FlightId()
        {
        }

        private FlightId()
        {
        }

        private FlightId(string segment1, string segment2, string segment3)
        {
            Segment1 = segment1;
            Segment2 = segment2;
            Segment3 = segment3;
        }

        public static FlightId From(string id)
        {
            var match = Regex.Match(id, @"^[a-zA-Z]{3}\s[0-9]{5}\s[a-zA-Z]{3}$");

            if (!match.Success)
            {
                throw new UnsupportedFlightIdException(id);
            }

            var flightId = new FlightId(id.Substring(0, 3), id.Substring(4, 5), id.Substring(10, 3));
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
            yield return Segment1;
            yield return Segment2;
            yield return Segment3;
        }
    }
}
