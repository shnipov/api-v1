using System.Collections.Generic;

namespace TodoApi.Models.RussianPost
{
    public class PostOffice
    {
        public int? Index { get; set; }
        public string Region { get; set; }
        public string Place { get; set; }
        public int? Parent { get; set; }
        public IEnumerable<string> AviaPort { get; set; }
        public int? AviaZone { get; set; }
    }
}