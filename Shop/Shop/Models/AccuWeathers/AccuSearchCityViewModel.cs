using System.ComponentModel.DataAnnotations;

namespace Shop.Models.AccuWeathers
{
    public class AccuSearchCityViewModel
    {
        
        public string City { get; set; }
        public string Country { get; set; }
        public double Temperature { get; set; }
        public double RealFeelTemperature { get; set; }
        public double RelativeHumidity { get; set; }
        public double Wind { get; set; }
        public double Pressure { get; set; }
        public string WeatherText { get; set; }
    }
}
