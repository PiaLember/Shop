namespace Shop.Models.AccuWeathers
{
    public class AccuWeatherViewModel
    {
        public string City { get; set; }
        public string EffectiveDate { get; set; }
        public Int64 EffectiveEpocDate { get; set; }
        public int Severity { get; set; }
        public string Text { get; set; }
        public string Category { get; set; }
        public string Enddate { get; set; }
        public Int64 EndEpocDate { get; set; }
        public double Minimum { get; set; }
        public double Maximum { get; set; }

    }
}
