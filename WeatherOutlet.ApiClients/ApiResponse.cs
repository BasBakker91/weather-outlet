using System.Net;

namespace WeatherOutlet.ApiClients
{
    public class ApiResponse<T>
    {
        public HttpStatusCode StatusCode { get; set; }
        public T Content { get; set; }

        public ApiResponse()
        {

        }

        public ApiResponse(HttpStatusCode statusCode, T content)
        {
            StatusCode = statusCode;
            Content = content;
        }
    }
}
