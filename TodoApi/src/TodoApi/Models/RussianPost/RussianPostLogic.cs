using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Extensions;
using Newtonsoft.Json;

namespace TodoApi.Models.RussianPost
{
    public class RussianPostLogic : IRussianPostLogic
    {
        public async Task<RussianPostCalculateResponse> Calculate(RussianPostCalculateRequest request)
        {
            using (var client = new HttpClient())
            {
                var uriBuilder = new UriBuilder("http", "tariff.russianpost.ru", 80, "tariff/v1/calculate");
                var queryBuilder = new QueryBuilder
                {
                    {request.ResponseFormat.ToString().ToLower(), string.Empty},
                    {"errorcode", request.ErrorCode ? "1" : "0" },
                    {"typ",  request.Typ.ToString()},
                    {"cat", request.Cat.ToString() },
                    {"dir", request.Dir.ToString() },
                    {"date", request.Date },
                    {"closed", request.Closed ? "1" : "0" },
                    {"from", request.From.ToString() },
                    {"to", request.To.ToString() },
                    {"country", request.Country.ToString() },
                    {"weight", request.Weight.ToString() },
                    {"sumoc", request.Sumoc.ToString() },
                    {"sumnp", request.Sumnp.ToString() },
                    {"isavia", request.IsAvia.ToString() },
                    {"service", request.Service.Select(x => x.ToString())}
                };
                uriBuilder.Query = queryBuilder.ToString();
                string uri = uriBuilder.ToString();

                HttpResponseMessage response = await client.GetAsync(uri);
                string responseString = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<RussianPostCalculateResponse>(responseString);
                return result;
            }
        }
    }
}