using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATSS.Domain.Entities
{
    public class FlightSchedule
    {
        public FlightSchedule()
        {
            Purchases = new List<FlightPurchase>();
        }

        public int Id { get; set; }
        public int Hour { get; set; }
        public int Minute { get; set; }
        public DayOfWeek Day { get; set; }
        public double Price { get; set; }

        public Flight Flight { get; set; }
        public IList<FlightPurchase> Purchases { get; private set; }
    }
}
