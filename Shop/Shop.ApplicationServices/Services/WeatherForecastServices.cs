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

        public async Task<AccuWeatherLocationResultDto> WeatherResult(AccuWeatherLocationResultDto dto)
        {
            string APIKey = "Ylo9qEmuGDlDcPTgMTImricoQ4A0itQq";
            string url = $"http://dataservice.accuweather.com/locations/v1/cities/search?apikey={APIKey}&q={dto.City}";
            //127964

            using (WebClient client = new WebClient())
            {
                string json = client.DownloadString(url);
                List<AccuWeatherLocationRootDto> accuResult = new JavaScriptSerializer().Deserialize<List<AccuWeatherLocationRootDto>>(json);

                dto.City = accuResult[0].LocalizedName;
                dto.Key = accuResult[0].Key;

            }
            string urlWeather = $"http://dataservice.accuweather.com/forecasts/v1/daily/1day/{dto.Key}?apikey={APIKey}&metric=true";


            using (WebClient client = new WebClient())
            {
                string json = client.DownloadString(urlWeather);
                WeatherRootDto result = new JavaScriptSerializer().Deserialize<WeatherRootDto>(json);

                var dailyForecast = result.DailyForecasts[0];

                dto.EffectiveDate = result.Headline.EffectiveDate.ToString("yyyy-MM-dd");
                dto.EffectiveEpochDate = result.Headline.EffectiveEpochDate;
                dto.Severity = result.Headline.Severity;
                dto.Text = result.Headline.Text;
                dto.Category = result.Headline.Category;
                dto.EndDate = result.Headline.EndDate.ToString("yyyy-MM-dd");
                dto.EndEpochDate = result.Headline.EndEpochDate;

                dto.MobileLink = result.Headline.MobileLink;
                dto.Link = result.Headline.Link;

                dto.DailyForecastsDate = result.DailyForecasts[0].Date.ToString("yyyy-MM-dd");
                dto.DailyForecastsEpochDate = result.DailyForecasts[0].EpochDate;

                dto.TempMinValue = result.DailyForecasts[0].Temperature.Minimum.Value;
                dto.TempMinUnit = result.DailyForecasts[0].Temperature.Minimum.Unit;
                dto.TempMinUnitType = result.DailyForecasts[0].Temperature.Minimum.UnitType;

                dto.TempMaxValue = result.DailyForecasts[0].Temperature.Maximum.Value;
                dto.TempMaxUnit = result.DailyForecasts[0].Temperature.Maximum.Unit;
                dto.TempMaxUnitType = result.DailyForecasts[0].Temperature.Maximum.UnitType;

                dto.DayIcon = result.DailyForecasts[0].Day.Icon;
                dto.DayIconPhrase = result.DailyForecasts[0].Day.IconPhrase;
                dto.DayHasPrecipitation = result.DailyForecasts[0].Day.HasPrecipitation;
                dto.DayPrecipitationType = result.DailyForecasts[0].Day.PrecipitationType;
                dto.DayPrecipitationIntensity = result.DailyForecasts[0].Day.PrecipitationIntensity;

                dto.NightIcon = result.DailyForecasts[0].Night.Icon;
                dto.NightIconPhrase = result.DailyForecasts[0].Night.IconPhrase;
                dto.NightHasPrecipitation = result.DailyForecasts[0].Night.HasPrecipitation;

            }
            return dto;
        }
    }
}
