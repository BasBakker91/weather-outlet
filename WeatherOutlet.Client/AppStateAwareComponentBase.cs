using Microsoft.AspNetCore.Blazor.Components;
using WeatherOutlet.Client.Services;

namespace WeatherOutlet.Client
{
    public class AppStateAwareComponentBase : BlazorComponent
    {
        [Inject]
        public AppState State { get; set; }

        protected override void OnInit()
        {
            State.OnAppStateChanged += StateHasChanged;
        }
    }
}
