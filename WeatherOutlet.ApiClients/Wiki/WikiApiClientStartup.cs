using Microsoft.Extensions.DependencyInjection;
using System;

namespace WeatherOutlet.ApiClients.Wiki
{
    public static class WikiApiClientStartup
    {
        /// <summary>
        /// Adds the wiki Api client to the <see cref="IServiceCollection"/>
        /// </summary>
        /// <param name="services">The service collection</param>
        public static IServiceCollection AddWikiApiClient(this IServiceCollection services)
        {
            services.AddHttpClient<IWikiApiClient, WikiApiClient>(config =>
            {
                config.BaseAddress = new Uri("https://en.wikipedia.org/");
                config.Timeout = TimeSpan.FromSeconds(30);
            });

            return services;
        }

    }
}
