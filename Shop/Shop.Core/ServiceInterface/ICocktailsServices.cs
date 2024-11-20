﻿using Shop.Core.Dto.CocktailsDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Core.ServiceInterface
{
    public interface ICocktailsServices
    {
       Task<List<CocktailsResultDto>> GetCocktails(CocktailsResultDto dto);
    }
}