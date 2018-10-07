using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using WeatherOutlet.Shared.Weather;

namespace WeatherOutlet.ApiClients.OpenWeather
{
    public class OpenWeatherApiClient : JsonApiClientBase, IOpenWeatherApiClient
    {
        private readonly OpenWeatherApiConfig openWeatherApiConfig;

        public OpenWeatherApiClient(HttpClient httpClient, OpenWeatherApiConfig openWeatherApiConfig) : base(httpClient) {
            this.openWeatherApiConfig = openWeatherApiConfig;

            if (string.IsNullOrWhiteSpace(openWeatherApiConfig.AppId))
                throw new InvalidOperationException("Can not use openWeather api without an appId");
        }

        public async Task<ApiResponse<WeatherForecast>> GetWeatherForecastAsync(string location) => await GetWeatherForecastAsync(location, CancellationToken.None);
        public async Task<ApiResponse<WeatherForecast>> GetWeatherForecastAsync(string location, CancellationToken cancellationToken)
        {
            try
            {
                var result = await GetAsync<WeatherForecast>(string.Format("2.5/forecast/daily?q={0}&type=accurate&mode=json&units=metric&cnt=7&lang={1}&appid={2}", location, openWeatherApiConfig.Language, openWeatherApiConfig.AppId), cancellationToken);

                return result;
            }
            catch(ApiException apiException)
            {
                if(apiException.StatusCode == HttpStatusCode.NotFound)
                {
                    return new ApiResponse<WeatherForecast>(HttpStatusCode.NotFound, null);
                }

                throw;
            }
        }
    }
}
