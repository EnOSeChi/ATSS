using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATSS.Domain.Entities
{
    public class FlightPurchase
    {
        public FlightPurchase()
        {
            Discounts = new List<PriceDiscount>();
        }

        public int Id { get; set; }
        public Tenant Tenant { get; set; }
        public FlightSchedule Flight { get; set; }
        public double Price { get; set; }
        public IList<PriceDiscount> Discounts { get; private set; }
    }
}
