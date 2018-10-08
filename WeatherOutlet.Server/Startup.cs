using Microsoft.AspNetCore.Blazor.Server;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using System;
using System.Linq;
using System.Net.Mime;
using WeatherOutlet.ApiClients.News;
using WeatherOutlet.ApiClients.OpenWeather;
using WeatherOutlet.ApiClients.Wiki;
using WeatherOutlet.Data;
using WeatherOutlet.Server.Settings;

namespace WeatherOutlet.Server
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(Configuration);

            IConfigurationSection openWeatherConfigurationSection = Configuration.GetSection(nameof(OpenWeatherSettings));
            IConfigurationSection applicationConfigurationSection = Configuration.GetSection(nameof(ApplicationSettings));
            IConfigurationSection newsConfigurationSection = Configuration.GetSection(nameof(NewsSettings));

            services.Configure<ApplicationSettings>(applicationConfigurationSection);
            services.Configure<OpenWeatherSettings>(openWeatherConfigurationSection);
            services.Configure<NewsSettings>(newsConfigurationSection);

            var openWeatherConfig = openWeatherConfigurationSection.Get<OpenWeatherSettings>();
            var newsConfig = newsConfigurationSection.Get<NewsSettings>();

            services.AddMvc().AddJsonOptions(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            });

            services.AddOpenWeatherApiClient(new OpenWeatherApiConfig()
            {
                AppId = openWeatherConfig.AppId,
                Language = openWeatherConfig.Language,
            });

            services.AddNewsApiClient(new NewsApiClientConfig()
            {
                ApiKey = newsConfig.ApiKey
            });

            services.AddWikiApiClient();

            services.AddHttpClient();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
            );

            services.AddResponseCompression(options =>
            {
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[]
                {
                    MediaTypeNames.Application.Octet,
                    WasmMediaTypeNames.Application.Wasm,
                });
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            try
            {
                using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    serviceScope.ServiceProvider.GetService<ApplicationDbContext>().Database.Migrate();
                }
            }
            catch (Exception)
            {
            }

            app.UseResponseCompression();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc(routes =>
            {
                routes.MapRoute(name: "default", template: "{controller}/{action}/{id?}");
            });

            app.UseBlazor<Client.Startup>();
        }
    }
}
