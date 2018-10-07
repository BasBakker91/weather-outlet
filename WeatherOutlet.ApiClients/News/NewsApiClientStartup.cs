using Microsoft.Extensions.DependencyInjection;
using System;

namespace WeatherOutlet.ApiClients.News
{
    public static class NewsApiClientStartup
    {
        public static IServiceCollection AddNewsApiClient(this IServiceCollection services, NewsApiClientConfig newsApiClientConfig)
        {
            // TODO: Build the config per request to change appId and/or language.
            services.AddSingleton(newsApiClientConfig);

            services.AddHttpClient<INewsApiClient, NewsApiClient>(config =>
            {
                config.BaseAddress = new Uri("https://newsapi.org/v2/");
                config.Timeout = TimeSpan.FromSeconds(30);
            });

            return services;
        }
    }
}
