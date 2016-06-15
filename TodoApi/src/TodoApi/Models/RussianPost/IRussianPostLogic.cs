using System.Threading.Tasks;

namespace TodoApi.Models.RussianPost
{
    /// <summary>
    /// Реализация формата обмена Почты РФ
    /// </summary>
    /// <remarks>
    /// Описание формата обмена http://tariff.russianpost.ru/TariffAPI.pdf
    /// </remarks>
    public interface IRussianPostLogic
    {
        Task<RussianPostCalculateResponse> Calculate(RussianPostCalculateRequest request);
    }
}