namespace TodoApi.Models
{
    public class Location
    {
        public string Id { get; set; }
        public string FiasCode { get; set; }
        public double Latitude { get; set; }
        public double Longtitude { get; set; }
        public Address Address { get; set; }
        public string Text { get; set; }
    }
}