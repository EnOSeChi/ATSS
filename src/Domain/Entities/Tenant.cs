using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATSS.Domain.Entities
{
    /// <summary>
    /// Tenant – system umożliwia użycie przez wielu klientów na raz. Takiego klienta określa się mianem
    /// tenant.Każdy tenant należy do jednej z grup A lub B.Uwaga! Istnieje różnica w funkcjonalnościach
    /// pomiędzy tenant-em typu A i B.
    /// </summary>
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
