using System;
using System.Collections.Generic;

namespace TodoApi.Models.RussianPost
{
    public class RussianPostCalculateException : Exception
    {
        public string Error { get; }

        public IEnumerable<string> Messages { get; }

        public string ApiUrl { get; set; }

        public RussianPostCalculateException(string error, List<string> messages, string apiUrl = null)
            : base(error + Environment.NewLine + string.Join(Environment.NewLine, messages))
        {
            Error = error;
            Messages = messages;
            ApiUrl = apiUrl;
        }
    }
}