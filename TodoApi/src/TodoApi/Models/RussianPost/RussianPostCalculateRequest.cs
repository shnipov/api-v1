using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models.RussianPost
{
    public class RussianPostCalculateRequest
    {
        public RussianPostCalculateRequest()
        {
            ResponseFormat = ResponseFormat.Json;
            ErrorCode = true;
            Date = DateTime.Now.ToString("yyyyMMdd");
            Closed = false;
            Service = new List<int>();
        }

        /// <summary>
        /// Формат запрашиваемых данных
        /// </summary>
        
        public ResponseFormat ResponseFormat { get; set; }

        /// <summary>
        /// Если в заголовке HTTP указано значение «1» (см.п. 1.3), возвращается код ошибки в диапазоне 400–599. Иначе возвращается код «200»
        /// </summary>
        public bool ErrorCode { get; set; }

        /// <summary>
        /// Тип почтового отправления, см. Приложение 1
        /// </summary>
        [Required]
        public int Typ { get; set; }

        /// <summary>
        /// Категория почтового отправления, см.Приложение 2
        /// </summary>
        [Required]
        public int Cat { get; set; }

        /// <summary>
        /// Направление доставки: внутреннее (указывается код «0») или международное (указывается код «1»)
        /// </summary>
        [Required]
        public int Dir { get; set; }

        /// <summary>
        /// Дата тарификации в виде YYYYMMDD, где YYYY – год, MM – месяц, DD – день
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// Признак тарификации при запрете доставки в указанную дату
        /// </summary>
        /// <remarks>
        /// Если доставка запрещена, то при значении «1» выводится соответствующее сообщение об ошибке
        /// </remarks>
        public bool Closed { get; set; }

        /// <summary>
        /// Индекс отправителя
        /// </summary>
        /// <remarks>
        /// Обязательно указывать для типов отправления (typ) 3, 4, 7, 15, 16
        /// </remarks>
        public int From { get; set; }

        /// <summary>
        /// Индекс получателя для внутренних отправлений
        /// </summary>
        /// <remarks>
        /// Обязательно указывать при внутреннем отправлении
        /// </remarks>
        public int To { get; set; }

        /// <summary>
        /// Страна получателя для международных отправлений см. Приложение 4
        /// </summary>
        /// <remarks>
        /// Обязательно указывать при международном отправлении
        /// </remarks>
        public int Country { get; set; }

        /// <summary>
        /// Вес (в граммах)
        /// </summary>
        /// <remarks>
        /// Обязательно указывать для типов отправлений (typ), тарификация которых зависит от веса
        /// </remarks>
        public int Weight { get; set; }

        /// <summary>
        /// Сумма объявленной ценности (в копейках)
        /// </summary>
        /// <remarks>
        /// Обязательно указывать для категорий отправления 2 и 4
        /// </remarks>
        public int Sumoc { get; set; }

        /// <summary>
        /// Сумма наложенного платежа (в копейках). В тарификации не участвует
        /// </summary>
        /// <remarks>
        /// При указании сумма наложенного платежа сравнивается c суммой объявленной ценности (если сумма наложенного платежа больше, выводится сообщение об ошибке)
        /// </remarks>
        public int Sumnp { get; set; }

        /// <summary>
        /// Предпочтительный вариант доставки*: 0 – наземная доставка, 1 – воздушная доставка по возможности, 2 – только воздушная доставка.
        /// </summary>
        public int IsAvia { get; set; }

        /// <summary>
        /// Указываются коды дополнительных услуг (см. Приложение 3), которые могут быть обрамлены в квадратные скобки
        /// </summary>
        public IEnumerable<int> Service { get; set; }
    }
}