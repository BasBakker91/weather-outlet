using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherOutlet.Shared.Weather
{
    public partial class WeatherForecast
    {
        public City City { get; set; }
        public Forecast[] List { get; set; }
    }
}
