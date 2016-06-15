namespace TodoApi.Models
{
    public class DeliveryTerms
    {
        public double MinWeight { get; set; }
        public double MaxWeight { get; set; }
        public Dimensions MinDimensions { get; set; }
        public Dimensions MaxDimensions { get; set; }
    }
}