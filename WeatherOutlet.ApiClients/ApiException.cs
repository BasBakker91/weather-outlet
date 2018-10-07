using System;
using System.Net;

namespace WeatherOutlet.ApiClients
{
    public class ApiException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Content { get; set; }

        public ApiException()
        {

        }

        public ApiException(HttpStatusCode statusCode, string content)
        {
            StatusCode = statusCode;
            Content = content;
        }
    }
}
