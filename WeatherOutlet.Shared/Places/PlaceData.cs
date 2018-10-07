using System;
using System.Collections.Generic;
using System.Text;
using WeatherOutlet.Shared.News;
using WeatherOutlet.Shared.Weather;

namespace WeatherOutlet.Shared.Places
{
    public class PlaceData
    {
        public string Place { get; set; }

        public WeatherForecast WeatherForecast { get; set; }
        public PlaceDetails PlaceDetails { get; set; }
        public List<Article> Articles { get; set; }

        public bool WeatherFound => WeatherForecast != null;
    }
}
