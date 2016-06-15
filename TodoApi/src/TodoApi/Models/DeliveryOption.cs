using System;
using System.Collections.Generic;

namespace TodoApi.Models
{
    public class DeliveryOption
    {
        public string DeliveryOptionKey { get; set; }
        public string DeliveryType { get; set; }
        public Price DeliveryPrice { get; set; }
        public DateTime ConfirmDate { get; set; }
        public DateTime PickupDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public IEnumerable<AddService> AddServices { get; set; }
        public DeliveryTerms DeliveryTerms { get; set; }
        public CancelTerms CancelTerms { get; set; }
        public UpdateTerms UpdateTerms { get; set; }
    }
}