using System.Collections.Generic;

namespace TodoApi.Models
{
    public class CalculateResponse
    {
        public IEnumerable<DeliveryOption> DeliveryOptions { get; set; }
    }
}