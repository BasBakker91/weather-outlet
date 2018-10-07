using Cloudcrate.AspNetCore.Blazor.Browser.Storage;
using Microsoft.AspNetCore.Blazor.Builder;
using Microsoft.Extensions.DependencyInjection;
using Sotsera.Blazor.Toaster.Core.Models;
using WeatherOutlet.Client.Services;

namespace WeatherOutlet.Client
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<AppState>();

            services.AddStorage();

            services.AddToaster(config =>
            {
                config.PositionClass = Defaults.Classes.Position.BottomRight;

                config.PreventDuplicates = true;
                config.NewestOnTop = false;

                config.ShowTransitionDuration = 500;
                config.VisibleStateDuration = 2000;
                config.HideTransitionDuration = 500;

                config.ProgressBarStepDuration = 40;

                config.HideStepDuration = 20;
                config.ShowStepDuration = 20;

            });
        }

        public void Configure(IBlazorApplicationBuilder app)
        {
            app.AddComponent<App>("app");
        }
    }
}
