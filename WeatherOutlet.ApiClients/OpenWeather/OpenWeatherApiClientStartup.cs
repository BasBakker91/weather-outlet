using Microsoft.Extensions.DependencyInjection;
using System;

namespace WeatherOutlet.ApiClients.OpenWeather
{
    public static class OpenWeatherApiClientStartup
    {
        /// <summary>
        /// Adds the news OpenWeather client to the <see cref="IServiceCollection"/>
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="openWeatherApiConfig">The api client config</param>
        public static IServiceCollection AddOpenWeatherApiClient(this IServiceCollection services, OpenWeatherApiConfig openWeatherApiConfig)
        {
            // TODO: Build the config per request to change appId and/or language.
            services.AddSingleton(openWeatherApiConfig);

            services.AddHttpClient<IOpenWeatherApiClient, OpenWeatherApiClient>(config =>
            {
                config.BaseAddress = new Uri("http://api.openweathermap.org/data/");
                config.Timeout = TimeSpan.FromSeconds(30);
            });

            return services;
        }

    }
}
