namespace WeatherOutlet.Shared.Weather
{
    public partial class Temp
    {
        public double Day { get; set; }
        public double Min { get; set; }
        public double Max { get; set; }
        public double Night { get; set; }
        public double Eve { get; set; }
        public double Morn { get; set; }

        public string MinimalDisplay => $"{(int)Min}°C";
        public string MaximumDisplay => $"{(int)Min}°C";
    }
}
