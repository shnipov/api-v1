namespace TodoApi.Models
{
    public class CalculateRequest
    {
        public Location From { get; set; }
        public Location To { get; set; }
        public Price DeclaredPrice { get; set; }
        public Price PaymentPrice { get; set; }
        public double Weight { get; set; }
        public Dimensions Dimensions { get; set; }
        public DeliveryFilter Filter { get; set; }
    }
}