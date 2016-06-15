using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.SwaggerGen.Annotations;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    /// <summary>
    /// Расчёт стоимости доставки
    /// </summary>
    [Route("api/[controller]")]
    public class CalculatorController : Controller
    {

        /// <summary>
        /// Расчитать варианты доставки
        /// </summary>
        /// <param name="request"></param>
        /// <remarks>
        /// Правила обработки параметров From и To:
        /// 1. Испольузется Id, если заполнено, остальные поля игнорируются;
        /// 2. Используется FiasCode, если заполнено, остальные поля игнорируются;
        /// 3. Используются координаты Latitude и Longtitude, если больше 0, остальные поля игнорируются;
        /// 4. Используются поля объекта Address. Обязательны для заполнения: [список полей];
        /// 5. Используется поле Text, парсим адрес в DaData.
        /// </remarks>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperationFilter(typeof(CalculateOperationFilter))]
        public async Task<CalculateResponse> Calculate([FromQuery]CalculateRequest request)
        {
            await Task.Delay(0);
            return new CalculateResponse
            {
                DeliveryOptions = new[]
                {
                    new DeliveryOption
                    {
                        DeliveryOptionKey = Guid.NewGuid().ToString(),
                        DeliveryType = "courier",
                        DeliveryPrice = new Price {Currency = "RU", Value = 300.0m},
                        ConfirmDate = DateTime.Now.AddHours(3),
                        PickupDate = DateTime.Now.AddHours(12),
                        DeliveryDate = DateTime.Now.AddDays(1)
                    },
                    new DeliveryOption
                    {
                        DeliveryOptionKey = Guid.NewGuid().ToString(),
                        DeliveryType = "pickup",
                        DeliveryPrice = new Price {Currency = "RU", Value = 300.0m},
                        ConfirmDate = DateTime.Now.AddHours(3),
                        PickupDate = DateTime.Now.AddHours(12),
                        DeliveryDate = DateTime.Now.AddDays(1)
                    }
                }
            };
        }
    }
}
