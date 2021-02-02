using ATSS.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATSS.Domain.Entities
{
    public class Flight
    {
        public Flight()
        {
            FlightSchedules = new List<FlightSchedule>();
        }

        public FlightId Id { get; private set; }
        public string From { get; set; }
        public string To { get; set; }
        public IList<FlightSchedule> FlightSchedules { get; private set; }
        public Tenant CreatedBy { get; set; }
    }
}
