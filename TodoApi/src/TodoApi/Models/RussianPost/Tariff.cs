namespace TodoApi.Models.RussianPost
{
    public class Tariff
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int? FromI { get; set; }
        public int? ToI { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public int? IsService { get; set; }
        public TariffValue Ground { get; set; }
        public TariffValue Avia { get; set; }
        public TariffValue Cover { get; set; }
        public TariffValue Service { get; set; }
    }
}