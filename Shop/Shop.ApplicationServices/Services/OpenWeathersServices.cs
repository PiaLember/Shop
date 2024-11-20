using Nancy.Json;
using Shop.Core.Dto.OpenWeatherDtos;
using Shop.Core.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Shop.ApplicationServices.Services
{
    public class OpenWeathersServices : IOpenWeathersServices
    {
        public async Task<OpenWeatherResultDto> OpenWeatherResult(OpenWeatherResultDto dto)
        {
            string apiKey = "ac0f4954f2276c6ad1120e7edce5fa23";
            string apiUrl = $"https://api.openweathermap.org/data/2.5/weather?q={dto.City}&units=metric&appid={apiKey}";

            using (WebClient client = new WebClient()) 
            {
                string json = client.DownloadString(apiUrl);

                OpenWeatherRootDto result = new JavaScriptSerializer().Deserialize<OpenWeatherRootDto>(json);

                dto.City = result.name;
                dto.Temp = result.main.temp;
                dto.FeelsLike = result.main.feels_like;
                dto.Humidity = result.main.humidity;
                dto.Pressure = result.main.pressure;
                dto.WindSpeed = result.wind.speed;
                dto.Conditions = result.weather[0].description;
                dto.Icon = result.weather[0].icon;
            }

            return null;
        }
    }
}
