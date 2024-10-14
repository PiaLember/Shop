using Shop.Core.Domain;
using Shop.Core.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Core.ServiceInterface
{
    public interface IRealEstatesServices
    {
        Task<RealEstate> Create(RealEstateDto dto);
        Task<RealEstate> DetailsAsync(Guid id);
        Task<RealEstate> Update(RealEstateDto dto);
        Task<RealEstate> Delete(Guid id);
    }
}
