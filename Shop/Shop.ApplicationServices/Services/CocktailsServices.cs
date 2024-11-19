using Nancy.Json;
using Shop.Core.Dto.CocktailsDtos;
using Shop.Core.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Shop.ApplicationServices.Services
{
    public class CocktailsServices : ICocktailsServices
    {
        public async Task<List<CocktailsResultDto>> GetCocktails(CocktailsResultDto dto)
        {
            string apiKey = "1";
            string apiUrl = $"https://www.thecocktaildb.com/api/json/v1/1/search.php?s={dto.strDrink}";

            using (WebClient client = new())
            {
                string json = client.DownloadString(apiUrl);
                CocktailsRootDto result = new JavaScriptSerializer().Deserialize<CocktailsRootDto>(json);

                if (result.drinks == null || result.drinks.Count == 0)
                {
                    return new List<CocktailsResultDto>();
                }

                var drinksList = result.drinks.Select(drink => new CocktailsResultDto
                {
                    idDrink = drink.idDrink,
                    strDrink = drink.strDrink,
                    strDrinkAlternate = drink.strDrinkAlternate,
                    strTags = drink.strTags,
                    strVideo = drink.strVideo,
                    strCategory = drink.strCategory,
                    strIBA = drink.strIBA,
                    strAlcoholic = drink.strAlcoholic,
                    strGlass = drink.strGlass,
                    strInstructions = drink.strInstructions,
                    strInstructionsES = drink.strInstructionsES,
                    strInstructionsDE = drink.strInstructionsDE,
                    strInstructionsFR = drink.strInstructionsFR,
                    strInstructionsIT = drink.strInstructionsIT,
                    strInstructionsZHHANS = drink.strInstructionsZHHANS,
                    strInstructionsZHHANT = drink.strInstructionsZHHANT,
                    strDrinkThumb = drink.strDrinkThumb,
                    strIngredient1 = drink.strIngredient1,
                    strIngredient2 = drink.strIngredient2,
                    strIngredient3 = drink.strIngredient3,
                    strIngredient4 = drink.strIngredient4,
                    strIngredient5 = drink.strIngredient5,
                    strIngredient6 = drink.strIngredient6,
                    strIngredient7 = drink.strIngredient7,
                    strIngredient8 = drink.strIngredient8,
                    strIngredient9 = drink.strIngredient9,
                    strIngredient10 = drink.strIngredient10,
                    strIngredient11 = drink.strIngredient11,
                    strIngredient12 = drink.strIngredient12,
                    strIngredient13 = drink.strIngredient13,
                    strIngredient14 = drink.strIngredient14,
                    strIngredient15 = drink.strIngredient15,
                    strMeasure1 = drink.strMeasure1,
                    strMeasure2 = drink.strMeasure2,
                    strMeasure3 = drink.strMeasure3,
                    strMeasure4 = drink.strMeasure4,
                    strMeasure5 = drink.strMeasure5,
                    strMeasure6 = drink.strMeasure6,
                    strMeasure7 = drink.strMeasure7,
                    strMeasure8 = drink.strMeasure8,
                    strMeasure9 = drink.strMeasure9,
                    strMeasure10 = drink.strMeasure10,
                    strMeasure11 = drink.strMeasure11,
                    strMeasure12 = drink.strMeasure12,
                    strMeasure13 = drink.strMeasure13,
                    strMeasure14 = drink.strMeasure14,
                    strMeasure15 = drink.strMeasure15,
                    strImageSource = drink.strImageSource,
                    strImageAttribution = drink.strImageAttribution,
                    strCreativeCommonsConfirmed = drink.strCreativeCommonsConfirmed,
                    dateModified = drink.dateModified
                }).ToList();

                return drinksList;
            }
        }
    }
}
