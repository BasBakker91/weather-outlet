namespace WeatherOutlet.Shared.Weather
{
    public partial class Weather
    {
        public long Id { get; set; }
        public string Main { get; set; }
        public string Icon { get; set; }

        public string ImagePath => string.IsNullOrWhiteSpace(Icon) ? string.Empty : $"{Id}{Icon[2]}.png";
    }
}
