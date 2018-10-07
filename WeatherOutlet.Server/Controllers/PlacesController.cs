using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WeatherOutlet.ApiClients;
using WeatherOutlet.ApiClients.News;
using WeatherOutlet.ApiClients.OpenWeather;
using WeatherOutlet.ApiClients.Wiki;
using WeatherOutlet.Shared.News;
using WeatherOutlet.Shared.Places;
using WeatherOutlet.Shared.Weather;

namespace WeatherOutlet.Server.Controllers
{
    [Route("api/[controller]")]
    public class PlacesController : ControllerBase
    {
        private readonly IOpenWeatherApiClient openWeatherApiClient;
        private readonly IWikiApiClient wikiApiClient;
        private readonly INewsApiClient newsApiClient;

        public PlacesController(IOpenWeatherApiClient openWeatherApiClient, IWikiApiClient wikiApiClient, INewsApiClient newsApiClient)
        {
            this.openWeatherApiClient = openWeatherApiClient;
            this.wikiApiClient = wikiApiClient;
            this.newsApiClient = newsApiClient;
        }

        [HttpGet("{city}")]
        public async Task<PlaceData> SearchAsync(string city)
        {
            PlaceData placeData = new PlaceData()
            {
                Place = city
            };

            var getWeatherTask = GetWeatherForecastAsync(city);
            var getPlaceDetailsTask = GetPlaceDetailsAsync(city);
            var getNewsTask = GetNewsAsync(city);

            await Task.WhenAll(getWeatherTask, getPlaceDetailsTask, getNewsTask);

            placeData.WeatherForecast = await getWeatherTask;
            placeData.PlaceDetails = await getPlaceDetailsTask;
            placeData.Articles = await getNewsTask;

            return placeData;
        }

        private async Task<WeatherForecast> GetWeatherForecastAsync(string city)
        {
            try
            {
                var result = await openWeatherApiClient.GetWeatherForecastAsync(city);
                return result.Content;
            }
            catch(ApiException)
            {
                return null;
            }
        }

        private async Task<PlaceDetails> GetPlaceDetailsAsync(string city)
        {
            try
            {
                var result = await wikiApiClient.GetWikiDetailsAsync(city);

                if (result == null || result.Content == null)
                    return null;

                return new PlaceDetails()
                {
                    Name = result.Content.Name,
                    Description = result.Content.Description,
                    WikipediaUrl = result.Content.Uri,
                };
            }
            catch (ApiException)
            {
                return null;
            }
        }

        private async Task<List<Article>> GetNewsAsync(string keyword)
        {
            try
            {
                var result = await newsApiClient.GetNewsAsync(keyword);

                if (result == null || result.Content == null)
                    return null;

                return (result.Content?.Articles?.Any(i => !string.IsNullOrWhiteSpace(i.Description)) ?? false) 
                    ? result.Content.Articles.Where(i => !string.IsNullOrWhiteSpace(i.Description)).ToList()
                    : null;
            }
            catch (ApiException)
            {
                return null;
            }
        }
    }
}