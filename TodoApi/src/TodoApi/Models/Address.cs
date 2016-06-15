namespace TodoApi.Models
{
    /// <summary>
    /// Адрес
    /// </summary>
    public class Address
    {
        /// <summary>
        /// Индекс
        /// </summary>
        public string PostalCode { get; set; }

        /// <summary>
        /// Страна
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Субъект
        /// </summary>
        public string Region { get; set; }

        /// <summary>
        /// Район
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// Город
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Район города
        /// </summary>
        public string CityDistrict { get; set; }

        /// <summary>
        /// Населённый пункт
        /// </summary>
        public string Settlement { get; set; }

        /// <summary>
        /// Улица
        /// </summary>
        public string Street { get; set; }

        /// <summary>
        /// Дом
        /// </summary>
        public string House { get; set; }

        /// <summary>
        /// Номер расширения дома
        /// </summary>
        public string Block { get; set; }

        /// <summary>
        /// Квартира
        /// </summary>
        public string Flat { get; set; }
    }
}