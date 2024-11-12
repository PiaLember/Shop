using Microsoft.AspNetCore.Mvc;
using Shop.Core.Dto.WeatherDtos;
using Shop.Core.ServiceInterface;
using Shop.Models.AccuWeathers;
using System.Runtime.CompilerServices;

namespace Shop.Controllers
{
    public class AccuWeathersController : Controller
    {
        private readonly IWeatherForecastServices _accuWeatherServices;

        public AccuWeathersController
            (IWeatherForecastServices accuWeathertServices)
        {
            _accuWeatherServices = accuWeathertServices;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SearchCity(AccuSearchCityViewModel model)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("City", "AccuWeathers", new { city = model.City });
            }
            return View(model);
        }

        public async Task<IActionResult> City(string city)
        {
            AccuWeatherLocationResultDto dto = new();
            WeatherRootDto dto1 = new();

            dto.City = city;

            await _accuWeatherServices.AccuWeatherResult(dto, dto1);

            AccuWeatherViewModel vm = new()
            {
                City = dto.City,
                EffectiveDate = dto.EffectiveDate,
                EffectiveEpocDate = dto.EffectiveEpochDate,
                Severity = dto.Severity,
                Text = dto.Text,
                Category = dto.Category,
                EndEpocDate = dto.EndEpochDate,
                Minimum = dto1.DailyForecasts[0].Temperature.Minimum.Value,
                Maximum = dto1.DailyForecasts[0].Temperature.Maximum.Value
            };

            return View(vm);
        }
    }
}
