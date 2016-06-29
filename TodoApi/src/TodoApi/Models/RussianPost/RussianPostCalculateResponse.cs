using System.Collections.Generic;
using Newtonsoft.Json;

namespace TodoApi.Models.RussianPost
{
    public class RussianPostCalculateResponse
    {
        /// <summary>
        /// Версия «Сервиса тарификации»
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Наименование отправления
        /// </summary>
        public string TypCatName { get; set; }

        /// <summary>
        /// Направление доставки: 0 – внутреннее направление; 1 – международное направление.
        /// </summary>
        public int? Dir { get; set; }

        /// <summary>
        /// Способ доставки: 2 – наземный; 3 – авиа; 4 – комбинированный
        /// </summary>
        public int? Trans { get; set; }

        /// <summary>
        /// Наименование способа доставки
        /// </summary>
        public string TransName { get; set; }

        /// <summary>
        /// Дата начала действия тарифа в виде YYYYMMDD, где YYYY – год, MM – месяц, DD – день
        /// </summary>
        [JsonProperty(PropertyName = "date-first")]
        public string DateFirst { get; set; }

        /// <summary>
        /// Список взимаемых тарифов. Содержит массив объектов тарифа
        /// </summary>
        public IEnumerable<Tariff> Tariff { get; set; }

        /// <summary>
        /// Список пунктов отправления и назначения
        /// </summary>
        public IEnumerable<PostOffice> PostOffice { get; set; }

        /// <summary>
        /// Итоговая сумма за почтовый сбор
        /// </summary>
        public TariffValue Ground { get; set; }

        /// <summary>
        /// Итоговая сумма за авиасбор
        /// </summary>
        public TariffValue Avia { get; set; }

        /// <summary>
        /// Итоговая страховка за объявленную ценность
        /// </summary>
        public TariffValue Cover { get; set; }

        /// <summary>
        /// Тариф за услугу
        /// </summary>
        public TariffValue Service { get; set; }

        /// <summary>
        /// Итоговая сумма платы с НДС в копейках
        /// </summary>
        public int? PayNds { get; set; }

        /// <summary>
        /// Итоговая сумма платы без НДС в копейках
        /// </summary>
        public int? Pay { get; set; }

        /// <summary>
        /// Итоговая сумма при оплате почтовыми марками в копейках (всегда кратна рублю)        /// </summary>
        public int? PayMark { get; set; }

        /// <summary>
        /// Список дополнительных услуг
        /// </summary>
        public IEnumerable<int> IsService { get; set; }

        /// <summary>
        /// Список объектов маршрута, описывающий маршру отправления. Состоит из объектов маршрута        /// </summary>
        public IEnumerable<Route> Route { get; set; }

        /// <summary>
        /// Запрос API
        /// </summary>
        public string ApiUrl { get; set; }
    }
}