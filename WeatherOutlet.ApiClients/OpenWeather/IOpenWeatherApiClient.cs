﻿using System.Threading;
using System.Threading.Tasks;
using WeatherOutlet.Shared.Weather;

namespace WeatherOutlet.ApiClients.OpenWeather
{
    public interface IOpenWeatherApiClient
    {
        Task<ApiResponse<WeatherForecast>> GetWeatherForecastAsync(string location);
        Task<ApiResponse<WeatherForecast>> GetWeatherForecastAsync(string location, CancellationToken cancellationToken);
    }
}