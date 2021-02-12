using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATSS.Domain.Entities
{
    /// <summary>
    /// zniżki
    /// </summary>
    public class PriceDiscount
    {
        public PriceDiscount()
        {
            Rules = new List<PriceDiscountRule>();
        }

        public int Id { get; set; }
        public IList<PriceDiscountRule> Rules { get; private set; }
    }
}
