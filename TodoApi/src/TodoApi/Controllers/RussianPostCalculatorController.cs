using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.SwaggerGen.Annotations;
using TodoApi.Models.RussianPost;
using Type = TodoApi.Models.RussianPost.Type;

// TODO Вынести контракты в отдельную сборку

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    public class RussianPostCalculatorController : Controller
    {
        private readonly IRussianPostLogic _russianPostLogic;

        public RussianPostCalculatorController(IRussianPostLogic russianPostLogic)
        {
            _russianPostLogic = russianPostLogic;
        }

        /// <summary>
        /// Рассчитать стоимость доставки Почтой РФ
        /// </summary>
        /// <param name="request"></param>
        /// <remarks>
        /// Допустимые значения параметров
        /// 
        /// Typ (приложение 1):
        /// - Не определено 0
        /// - Бланк почтового перевода 1
        /// - Письмо 2
        /// - Бандероль 3
        /// - Посылка 4
        /// - Мелкий пакет 5
        /// - Почтовая карточка 6
        /// - Отправление EMS 7
        /// - Секограмма 8
        /// - Мешок «М» 9
        /// - резерв 10
        /// - Бланк уведомления 12
        /// - Газетная пачка 13
        /// - Сгруппированные отправления
        /// - «Консигнация» 14
        /// - Письмо 1 класса 15
        /// - Бандероль 1 класса 16
        /// - Бланк уведомления 1 класса 17
        /// - Сумка страховая 18
        /// - ОВПО – письмо 19
        /// - Мультиконверт 20
        /// - Тяжеловесное почтовое отправление 21
        /// - ОВПО - карточка 22
        /// - Посылка онлайн 23
        /// - Курьер онлайн 24
        /// - Отправление ДМ 25
        /// - Пакет ДМ 26
        /// - Посылка стандарт 27
        /// - Посылка курьер 28
        /// - Посылка экспресс 29
        /// - Бизнес курьер 30
        /// - Бизнес курьер экспресс 31
        /// 
        /// Cat (приложение 2):
        /// - Простое 0 (используется для идентификации входящих из-за границы международных мелких пакетов категории «простое» и направляемых в РФ)
        /// - Заказное 1
        /// - С объявленной ценностью 2
        /// - Обыкновенное 3
        /// - С объявленной ценностью и наложенным платежом 4
        /// - Не определена 5
        /// - С объявленной ценностью и обязательным платежом 6
        /// 
        /// Service (приложение 3):
        /// - Уведомление о вручении 1
        /// - Заказное уведомление о вручении 2
        /// - Опись вложений 3
        /// - Отметка «Осторожно/Хрупкая» 4
        /// - Громоздкая посылка 6
        /// - Вручение нарочным 7
        /// - Доставка документов 9
        /// - Доставка товаров 10
        /// - Нестандартный размер 12
        /// - Застраховано 14
        /// - Оплата наложенного платежа отправителем 24
        /// - SMS-уведомление о поступлении 20
        /// - SMS-уведомление о вручении 21
        /// - Проверка соответствия вложения почтового отправления описи вложения 22
        /// - Составление описи вложения почтового отправления с объявленной ценностью 23
        /// 
        /// Country (приложение 4):
        /// http://tariff.russianpost.ru/tariff/v1/dictionary?jsontext%26country
        /// </remarks>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, Description = "Результат расчёта", Type = typeof(RussianPostCalculateResponse))]
        [SwaggerResponse(HttpStatusCode.BadRequest, Description = "Сообщение об ошибках", Type = typeof(string))]
        public async Task<IActionResult> Calculate([FromQuery] RussianPostCalculateRequest request)
        {
            try
            {
                return Ok(await _russianPostLogic.Calculate(request));
            }
            catch (Exception ex)
            {
                return ExceptionHandling.Handle(this, ex);
            }
        }

        /// <summary>
        /// Рассчитать стоимость по тарифу "Посылка"
        /// </summary>
        [HttpGet("Parcel")]
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, Description = "Результат расчёта")]
        [SwaggerResponse(HttpStatusCode.BadRequest, Description = "Сообщение об ошибках", Type = typeof(string))]
        public async Task<IActionResult> Calculate([FromQuery] RussianPostParcelCalculateRequest request)
        {
            try
            {
                Category cat = GetRussianPostCategory(request.Sumoc, request.Sumnp);

                var response = await _russianPostLogic.Calculate(new RussianPostCalculateRequest
                {
                    Typ = (int) Type.Parcel,
                    Cat = (int) cat,
                    Dir = (int) Direction.Internal,
                    From = request.From,
                    To = request.To,
                    Weight = request.Weight,
                    Sumoc = request.Sumoc,
                    Sumnp = request.Sumnp,
                    IsAvia = (int?) Avia.No
                });

                return Ok(new
                {
                    Name = response.TypCatName,
                    PriceNoNds = response.Pay / 100.00m,
                    PriceWithDns = response.PayNds / 100.0m,
                    response.ApiUrl
                });
            }
            catch (Exception ex)
            {
                return ExceptionHandling.Handle(this, ex);
            }
        }

        /// <summary>
        /// Рассчитать стоимость по тарифу "Посылка Онлайн"
        /// </summary>
        [HttpGet("ParcelOnline")]
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, Description = "Результат расчёта")]
        [SwaggerResponse(HttpStatusCode.BadRequest, Description = "Сообщение об ошибках", Type = typeof(string))]
        public async Task<IActionResult> Calculate([FromQuery] RussianPostParcelOnlineCalculateRequest request)
        {
            try
            {
                Category cat = GetRussianPostCategory(request.Sumoc, request.Sumnp);

                var response = await _russianPostLogic.Calculate(new RussianPostCalculateRequest
                {
                    Typ = (int)Type.ParcelOnline,
                    Cat = (int)cat,
                    Dir = (int)Direction.Internal,
                    From = request.From,
                    To = request.To,
                    Sumoc = request.Sumoc,
                    Sumnp = request.Sumnp,
                    IsAvia = (int?)Avia.No
                });

                return Ok(new
                {
                    Name = response.TypCatName,
                    PriceNoNds = response.Pay / 100.00m,
                    PriceWithDns = response.PayNds / 100.0m,
                    response.ApiUrl
                });
            }
            catch (Exception ex)
            {
                return ExceptionHandling.Handle(this, ex);
            }
        }

        /// <summary>
        /// Рассчитать стоимость по тарифу "Курьер Онлайн"
        /// </summary>
        [HttpGet("CourierOnline")]
        [Authorize("Token")]
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, Description = "Результат расчёта")]
        [SwaggerResponse(HttpStatusCode.BadRequest, Description = "Сообщение об ошибках", Type = typeof(string))]
        public async Task<IActionResult> Calculate([FromQuery] RussianPostCourierOnlineCalculateRequest request)
        {
            try
            {
                Category cat = GetRussianPostCategory(request.Sumoc, null);

                var response = await _russianPostLogic.Calculate(new RussianPostCalculateRequest
                {
                    Typ = (int)Type.CourierOnline,
                    Cat = (int)cat,
                    Dir = (int)Direction.Internal,
                    From = request.From,
                    To = request.To,
                    Sumoc = request.Sumoc,
                    IsAvia = (int?)Avia.No
                });

                return Ok(new
                {
                    Name = response.TypCatName,
                    PriceNoNds = response.Pay / 100.00m,
                    PriceWithDns = response.PayNds / 100.0m,
                    response.ApiUrl
                });
            }
            catch (Exception ex)
            {
                return ExceptionHandling.Handle(this, ex);
            }
        }

        private static Category GetRussianPostCategory(int? sumoc, int? sumnp)
        {
            Category cat = Category.Parcel;
            if (sumoc.HasValue && sumoc.Value > 0)
            {
                if (sumnp.HasValue && sumnp.Value > 0)
                {
                    cat = Category.ParcelWithPaymentPrice;
                }
                else
                {
                    cat = Category.ParcelWithDeclaredPrice;
                }
            }
            else
            {
                if (sumnp.HasValue && sumnp.Value > 0)
                {
                    throw new RussianPostCalculateException("Необходимо указать Sumoc", new List<string>());
                }
            }
            return cat;
        }
    }
}
