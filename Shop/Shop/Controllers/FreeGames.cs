using Microsoft.AspNetCore.Mvc;
using Shop.Core.Dto.FreeGamesDtos;
using Shop.Core.ServiceInterface;
using Shop.Models.FreeGames;

namespace Shop.Controllers
{
    public class FreeGames : Controller
    {
        private readonly IFreeGamesServices _freeGamesServices;

        public FreeGames
            (
            IFreeGamesServices freeGamesServices
            )
        {
            _freeGamesServices = freeGamesServices;
        }
        public IActionResult Index()
        {
            FreeGamesResultDto dto = new();
            _freeGamesServices.FreeGamesResult(dto);
            var vmList = dto.FreeGames.Select(game => new IndexViewModel
            {
                id = game.id,
                title = game.title,
                thumbnail = game.thumbnail,
                short_description = game.short_description,
                game_url = game.game_url,
                genre = game.genre,
                platform = game.platform,
                publisher = game.publisher,
                developer = game.developer,
                release_date = game.release_date,
                freetogame_profile_url = game.freetogame_profile_url
            }).ToList();

            return View(vmList);
        }

        [HttpPost]
        public IActionResult ShowFreeGames(IndexViewModel model)
        {
            return View(model);
        }
    }
}
