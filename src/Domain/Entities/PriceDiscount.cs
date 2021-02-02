using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATSS.Domain.Entities
{
    public class PriceDiscount
    {
        public int Id { get; set; }
        public Func<FlightSchedule, Tenant, bool> Predicate { get; set; }
    }
}
