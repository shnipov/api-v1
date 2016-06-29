namespace TodoApi.Models.RussianPost
{
    public class Tariff
    {
        /// <summary>
        /// Код тарифа
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Наименование тарифа
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Индекс места отправления
        /// </summary>
        public int? FromI { get; set; }

        /// <summary>
        /// Индекс места назначения
        /// </summary>
        public int? ToI { get; set; }

        /// <summary>
        /// Наименование места отправления
        /// </summary>
        public string From { get; set; }

        /// <summary>
        /// Наименование места назначения        /// </summary>
        public string To { get; set; }

        /// <summary>
        /// Код услуги
        /// </summary>
        public int? IsService { get; set; }

        /// <summary>
        /// Сумма за почтовый сбор
        /// </summary>
        public TariffValue Ground { get; set; }

        /// <summary>
        /// Сумма за авиасбор
        /// </summary>
        public TariffValue Avia { get; set; }

        /// <summary>
        /// Страховка за объявленную ценность
        /// </summary>
        public TariffValue Cover { get; set; }

        /// <summary>
        /// Тариф за услугу
        /// </summary>
        public TariffValue Service { get; set; }
    }
}