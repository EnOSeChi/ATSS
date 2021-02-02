using ATSS.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATSS.Domain.Entities
{
    /// <summary>
    /// Flight - oprócz ID zawiera trasę od, do, godzinę i dni tygodnia wylotu.
    /// </summary>
    public class Flight
    {
        public Flight()
        {
            CustomPrices = new List<FlightPrice>();
        }

        public FlightId Id { get; private set; }
        public string From { get; set; }
        public string To { get; set; }
        public double DefaultPrice { get; set; }    
        public int Hour { get; set; }
        public int Minute { get; set; }
        public bool Monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednesday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }
        public bool Saturday { get; set; }
        public bool Sunday { get; set; }
        public Tenant CreatedBy { get; set; }
        public IList<FlightPrice> CustomPrices { get; set; }
    }
}
