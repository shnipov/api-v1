using System.Collections.Generic;
using Newtonsoft.Json;

namespace TodoApi.Models.RussianPost
{
    public class RussianPostCalculateResponse
    {
        public string Version { get; set; }

        public string TypCatName { get; set; }

        public int? Dir { get; set; }

        public int? Trans { get; set; }

        public string TransName { get; set; }

        [JsonProperty(PropertyName = "date-first")]
        public string DateFirst { get; set; }

        public IEnumerable<Tariff> Tariff { get; set; }

        public IEnumerable<PostOffice> PostOffice { get; set; }

        public TariffValue Ground { get; set; }

        public TariffValue Avia { get; set; }

        public TariffValue Cover { get; set; }

        public TariffValue Service { get; set; }

        public int? PayNds { get; set; }

        public int? Pay { get; set; }

        public int? PayMark { get; set; }

        public IEnumerable<int> IsService { get; set; }

        public IEnumerable<Route> Route { get; set; }

    }
}