using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATSS.Domain.Entities
{
    /// <summary>
    /// warunki jakie musza być spełnione dla zniżki
    /// </summary>
    public class PriceDiscountRule
    {
        public int Id { get; set; }
        public string FieldName { get; set; }
        public string Value { get; set; }
        public PriceDiscount PriceDiscount { get; set; }
    }
}
