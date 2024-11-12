using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Nancy.Json;
using Shop.Core.Dto.WeatherDtos;
using Shop.Core.ServiceInterface;


namespace Shop.ApplicationServices.Services
{
    public class WeatherForecastServices : IWeatherForecastServices
    {
        string APIKey = "Ylo9qEmuGDlDcPTgMTImricoQ4A0itQq";

        public async Task<AccuWeatherLocationResultDto> AccuWeatherResult(AccuWeatherLocationResultDto dto, WeatherRootDto dto1)
        {
            
            string url = $"http://dataservice.accuweather.com/locations/v1/cities/search?apikey={APIKey}&q={dto.City}";

          
                using (WebClient client = new WebClient())
                {
                    string json = client.DownloadString(url);
                    List<AccuWeatherLocationRootDto> accuResult = new JavaScriptSerializer().Deserialize<List<AccuWeatherLocationRootDto>>(json);

                    dto.City = accuResult[0].LocalizedName;
                    dto.Key = accuResult[0].Key;
                  
                }

            string urlWeather = $"http://dataservice.accuweather.com/forecasts/v1/daily/1day/{dto.Key}?apikey={APIKey}q&metric=true";

            using (WebClient client = new WebClient())
            {
                string json = client.DownloadString(urlWeather);
                WeatherRootDto accuResult = new JavaScriptSerializer().Deserialize<WeatherRootDto>(json);

                var dailyForecast = accuResult.DailyForecasts[0];

                dto.EffectiveDate = accuResult.Headline.EffectiveDate.ToString("yyyy-MM-dd");
                dto.EffectiveEpochDate = accuResult.Headline.EffectiveEpochDate;
                dto.Severity = accuResult.Headline.Severity;
                dto.Text = accuResult.Headline.Text;
                dto.Date = dailyForecast.Date;
                dto.Temperature = dailyForecast.Temperature;
                dto.Day = dailyForecast.Day;
                dto.Night = dailyForecast.Night;
                dto.Sources = dailyForecast.Sources;
                dto.MobileLink = dailyForecast.MobileLink;
                dto.Link = dailyForecast.Link;
                dto.Icon = dailyForecast.Day.Icon;
                dto.IconPhrase = dailyForecast.Day.IconPhrase;
                dto.HasPrecipitation = dailyForecast.Day.HasPrecipitation;
                dto.Minimum = dailyForecast.Temperature.Minimum;
                dto.Maximum = dailyForecast.Temperature.Maximum;
                






            }

            return dto;
        }
    }
}
