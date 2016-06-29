namespace TodoApi.Models.RussianPost
{
    public class Route
    {
        /// <summary>
        /// Способ пересылки (аналогично общим параметрам)
        /// </summary>
        public int? Trans { get; set; }

        /// <summary>
        /// Индекс места отправления
        /// </summary>
        public int? FromPostOffice { get; set; }

        /// <summary>
        /// Код аэропорта отправления
        /// </summary>
        public int? FromAviaPort { get; set; }

        /// <summary>
        /// Код страны назначения
        /// </summary>
        public int? ToCountry { get; set; }

        /// <summary>
        /// Индекс места назначения
        /// </summary>
        public int? ToPostOffice { get; set; }

        /// <summary>
        /// Код аэропорта назначения
        /// </summary>
        public int? ToAviaPort { get; set; }

        /// <summary>
        /// Признак доставки с учетом ограничений при значении true
        /// </summary>
        public bool? Limit { get; set; }
    }
}