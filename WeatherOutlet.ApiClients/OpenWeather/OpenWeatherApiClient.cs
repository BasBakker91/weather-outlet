using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using WeatherOutlet.Shared.Weather;

namespace WeatherOutlet.ApiClients.OpenWeather
{
    /// <summary>
    /// An Api Client that interacts with <a href="https://openweathermap.org/api">OpenWeather's Api</a>
    /// </summary>
    public class OpenWeatherApiClient : JsonApiClientBase, IOpenWeatherApiClient
    {
        private readonly OpenWeatherApiConfig openWeatherApiConfig;

        /// <summary>
        /// Creates an instance of <see cref="OpenWeatherApiClient"/>
        /// </summary>
        public OpenWeatherApiClient(HttpClient httpClient, OpenWeatherApiConfig openWeatherApiConfig) : base(httpClient) {
            this.openWeatherApiConfig = openWeatherApiConfig;
            

            if (string.IsNullOrWhiteSpace(openWeatherApiConfig.AppId))
                throw new InvalidOperationException("Can not use openWeather api without an appId");
        }

        public OpenWeatherApiClient() : base(new HttpClient())
        {

        }

        /// <summary>
        /// Gets the weather forecast for a certain location.
        /// </summary>
        /// <param name="location">Location has to match the city exactly.</param>
        /// <returns></returns>
        public virtual async Task<ApiResponse<WeatherForecast>> GetWeatherForecastAsync(string location)
        {
            try
            {
                var uri = $"2.5/forecast/daily?q={location}&type=accurate&mode=json&units=metric&cnt=7&lang={openWeatherApiConfig.Language}&appid={openWeatherApiConfig.AppId}";

                var result = await GetAsync<WeatherForecast>(uri);

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
