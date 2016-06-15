namespace TodoApi.Models.RussianPost
{
    public class Route
    {
        public int? Trans { get; set; }
        public int? FromPostOffice { get; set; }
        public int? FromAviaPort { get; set; }
        public int? ToCountry { get; set; }
        public int? ToPostOffice { get; set; }
        public int? ToAviaPort { get; set; }
        public bool? Limit { get; set; }
    }
}