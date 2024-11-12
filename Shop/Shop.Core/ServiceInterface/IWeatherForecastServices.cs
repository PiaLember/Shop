using Shop.Core.Dto.WeatherDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Core.ServiceInterface
{
    public interface IWeatherForecastServices
    {
        Task<AccuWeatherLocationResultDto> AccuWeatherResult(AccuWeatherLocationResultDto dto, WeatherRootDto dto1);
    }
}
