using Microsoft.Extensions.DependencyInjection;
using System;

namespace WeatherOutlet.ApiClients.News
{
    public static class NewsApiClientStartup
    {
        /// <summary>
        /// Adds the news Api client to the <see cref="IServiceCollection"/>
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="newsApiClientConfig">The api client config</param>
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
