using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecruitmentTask.Domain.Entities
{
    public class Tenant
    {
        public Tenant()
        {
            CreatedFlights = new List<Flight>();
            PurchasedFlights = new List<FlightPurchase>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public TenantGroup TenantGroup { get; set; }
        public IList<Flight> CreatedFlights { get; private set; }
        public IList<FlightPurchase> PurchasedFlights { get; private set; }
    }
}
