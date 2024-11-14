﻿using Microsoft.AspNetCore.Mvc;
using Shop.Core.Dto.FreeGamesDtos;
using Shop.Core.ServiceInterface;
using Shop.Models.FreeGames;
using System.Linq;

namespace Shop.Controllers
{
    public class FreeGames : Controller
    {
        private readonly IFreeGamesServices _freeGamesServices;
        private const int PageSize = 10;

        public FreeGames
            (
            IFreeGamesServices freeGamesServices
            )
        {
            _freeGamesServices = freeGamesServices;
        }
        public async Task<IActionResult> Index(string sortOrder, string searchString, int pageNumber = 1)
        {
            FreeGamesResultDto dto = new();
            await _freeGamesServices.FreeGamesResult(dto);

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
            });



            if (!string.IsNullOrEmpty(searchString))
            {
                vmList = vmList.Where(g => g.title.Contains(searchString, StringComparison.OrdinalIgnoreCase));
            }

            vmList = sortOrder switch
            {
                "name_desc" => vmList.OrderByDescending(g => g.title),
                "name_asc" => vmList.OrderBy(g => g.title),
                "genre_desc" => vmList.OrderByDescending(g => g.genre),
                "genre_asc" => vmList.OrderBy(g => g.genre),
                "platform_desc" => vmList.OrderByDescending(g => g.platform),
                "platform_asc" => vmList.OrderBy(g => g.platform),
                "release_date_desc" => vmList.OrderByDescending(g => g.release_date),
                "release_date_asc" => vmList.OrderBy(g => g.release_date),
                _ => vmList.OrderBy(g => g.title)
            };

            int totalItems = vmList.Count();
            vmList = vmList.Skip((pageNumber - 1) * PageSize).Take(PageSize);

            // Pass pagination data to the view
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / PageSize);
            ViewBag.CurrentPage = pageNumber;
            ViewBag.SortOrder = sortOrder;
            ViewBag.SearchString = searchString;

            return View(vmList.ToList());
        }


        [HttpPost]
        public IActionResult ShowFreeGames(IndexViewModel model)
        {
            return View(model);
        }
    }
}
