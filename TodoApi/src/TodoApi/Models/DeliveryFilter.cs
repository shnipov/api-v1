using System.Collections.Generic;

namespace TodoApi.Models
{
    public class DeliveryFilter
    {
        public IEnumerable<string> DeliveryTypes { get; set; }
        public IEnumerable<string> PaymentTypes { get; set; }
        public IEnumerable<string> AddServices { get; set; }
    }
}