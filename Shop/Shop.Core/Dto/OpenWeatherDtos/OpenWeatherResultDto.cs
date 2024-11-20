﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Core.Dto.OpenWeatherDtos
{
    public class OpenWeatherResultDto
    {
        public string City { get; set; }
        public double Temp {  get; set; }
        public double FeelsLike { get; set; }
        public int Humidity { get; set; }
        public int Pressure { get; set; }
        public double WindSpeed { get; set; }
        public string Conditions { get; set; }
    }
}
