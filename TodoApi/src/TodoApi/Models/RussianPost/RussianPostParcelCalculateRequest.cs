namespace TodoApi.Models.RussianPost
{
    public class RussianPostParcelCalculateRequest
    {
        public int? From { get; set; }
        public int? To { get; set; }
        public int? Sumoc { get; set; }
        public int? Sumnp { get; set; }
        public int? Weight { get; set; }
    }
}