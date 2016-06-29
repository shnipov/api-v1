using System;
using System.Collections.Generic;
using System.Linq;
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
                string queryString = GetQueryString(request);
                uriBuilder.Query = queryString;
                string uri = uriBuilder.ToString();

                HttpResponseMessage response = await client.GetAsync(uri);

                if (!response.IsSuccessStatusCode)
                {
                    string errorResponseString = await response.Content.ReadAsStringAsync();
                    var error = JsonConvert.DeserializeAnonymousType(
                        errorResponseString,
                        new {Caption = string.Empty, Error = new List<string>()});
                    throw new RussianPostCalculateException(error.Caption, error.Error, uri);
                }

                string responseString = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<RussianPostCalculateResponse>(responseString);
                result.ApiUrl = uri;

                return result;
            }
        }

        private static string GetQueryString(RussianPostCalculateRequest request)
        {
            var queryBuilder = new QueryBuilder {{request.ResponseFormat.ToString().ToLower(), string.Empty}};
            if (request.ErrorCode.HasValue)
            {
                queryBuilder.Add("errorcode", request.ErrorCode.Value ? "1" : "0");
            }
            if (request.Typ.HasValue)
            {
                queryBuilder.Add("typ", request.Typ.ToString());
            }
            if (request.Cat.HasValue)
            {
                queryBuilder.Add("cat", request.Cat.ToString());
            }
            if (request.Dir.HasValue)
            {
                queryBuilder.Add("dir", request.Dir.ToString());
            }
            if (!string.IsNullOrWhiteSpace(request.Date))
            {
                queryBuilder.Add("date", request.Date);
            }
            if (request.Closed.HasValue)
            {
                queryBuilder.Add("closed", request.Closed.Value ? "1" : "0");
            }
            if (request.From.HasValue)
            {
                queryBuilder.Add("from", request.From.ToString());
            }
            if (request.To.HasValue)
            {
                queryBuilder.Add("to", request.To.ToString());
            }
            if (request.Country.HasValue)
            {
                queryBuilder.Add("country", request.Country.ToString());
            }
            if (request.Weight.HasValue)
            {
                queryBuilder.Add("weight", request.Weight.ToString());
            }
            if (request.Sumoc.HasValue)
            {
                queryBuilder.Add("sumoc", request.Sumoc.ToString());
            }
            if (request.Sumnp.HasValue)
            {
                queryBuilder.Add("sumnp", request.Sumnp.ToString());
            }
            if (request.IsAvia.HasValue)
            {
                queryBuilder.Add("isavia", request.IsAvia.ToString());
            }
            if (request.Service != null && request.Service.Any())
            {
                queryBuilder.Add("service", string.Join(",", request.Service));
            }
            return queryBuilder.ToString();
        }
    }
}