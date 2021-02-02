using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATSS.Domain.Entities
{
    /// <summary>
    /// Flight price – cena lotu, która może być różna dla różnych dat i godzin
    /// </summary>
    public class FlightPrice
    {
        public int Id { get; set; }
        public DateTimeOffset From { get; set; }
        public DateTimeOffset To { get; set; }
        public double Value { get; set; }

        public Flight Flight { get; set; }
    }
}
