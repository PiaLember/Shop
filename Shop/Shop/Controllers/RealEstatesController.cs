using Microsoft.AspNetCore.Mvc;
using Shop.ApplicationServices.Services;
using Shop.Core.ServiceInterface;
using Shop.Data;
using Shop.Models.RealEstates;

namespace Shop.Controllers
{
    public class RealEstatesController : Controller
    {

        private readonly ShopContext _context;
        private readonly IRealEstatesServices _realEstatesServices;
        private readonly IFileServices _fileServices;

        public RealEstatesController
        (
            ShopContext context,
            IRealEstatesServices RealEstatesServices,
            IFileServices fileServices
        )
        {
            _context = context;
            _realEstatesServices = RealEstatesServices;
            _fileServices = fileServices;

        }
        public IActionResult Index()
        {
            var result = _context.RealEstates
                .Select(x => new RealEstatesIndexViewModel
                {
                    Id = x.Id,
                    Location = x.Location,
                    BuildingType = x.BuildingType,
                    Size = x.Size,

                });
            return View(result);
        }
    }
}
