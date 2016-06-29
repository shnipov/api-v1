namespace TodoApi.Models.RussianPost
{
    public class TariffValue
    {
        /// <summary>
        /// Значение тарифа без НДС в копейках
        /// </summary>
        public int? Val { get; set; }

        /// <summary>
        /// Значение тарифа с НДС в копейках
        /// </summary>
        public int? ValNds { get; set; }

        /// <summary>
        /// Значение тарифа при оплате почтовыми марками        /// </summary>
        public int? ValMark { get; set; }
    }
}