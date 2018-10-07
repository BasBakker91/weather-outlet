using System;

namespace WeatherOutlet.Shared.Weather
{
    public partial class Forecast
    {
        public long Dt { get; set; }
        public Temp Temp { get; set; }
        public Weather[] Weather { get; set; }
        public double? Rain { get; set; }

        public DateTime DateTime => DateTimeOffset.FromUnixTimeSeconds(Dt).DateTime;
        public string DayOfWeek => DateTime.DayOfWeek.ToString();
    }
}
