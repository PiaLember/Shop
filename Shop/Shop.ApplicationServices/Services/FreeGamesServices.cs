using Nancy.Json;
using Shop.Core.Dto.ChuckNorrisDtos;
using Shop.Core.Dto.FreeGamesDtos;
using Shop.Core.Dto.WeatherDtos;
using Shop.Core.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Shop.ApplicationServices.Services
{
    public class FreeGamesServices : IFreeGamesServices
    {
        public async Task<FreeGamesResultDto> FreeGamesResult(FreeGamesResultDto dto)
        {
            var url = "https://www.freetogame.com/api/games?platform=pc";

            using (WebClient client = new WebClient())
            {
                string json = client.DownloadString(url);
                List<FreeGamesRootDto> freeGameResult = JsonSerializer.Deserialize<List<FreeGamesRootDto>>(json);

                if (freeGameResult != null)
                {
                    dto.FreeGames = freeGameResult;
                }
            }

            return dto;
        }
    }
}
