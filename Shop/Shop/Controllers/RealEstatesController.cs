using Microsoft.AspNetCore.Mvc;
using Shop.ApplicationServices.Services;
using Shop.Core.Dto;
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

        [HttpGet]
        public IActionResult Create()
        {
            RealEstateCreateUpdateViewModel result = new RealEstateCreateUpdateViewModel();

            return View("CreateUpdate", result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(RealEstateCreateUpdateViewModel vm)
        {
            var dto = new RealEstateDto()
            {
                Id = vm.Id,
                Location = vm.Location,
                Size = vm.Size,
                RoomNumber = vm.RoomNumber,
                BuildingType = vm.BuildingType,
                CreatedAt = vm.CreatedAt,
                UpdatedAt = vm.UpdatedAt,
            
            };

            var result = await _realEstatesServices.Create(dto);

            if (result == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index), vm);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var RealEstate = await _realEstatesServices.DetailsAsync(id);

            if (RealEstate == null)
            {
                return NotFound();
            }

            var vm = new RealEstateDetailsViewModel();

            vm.Id = RealEstate.Id;
            vm.Location = RealEstate.Location;
            vm.Size = RealEstate.Size;
            vm.RoomNumber = RealEstate.RoomNumber;
            vm.BuildingType = RealEstate.BuildingType;
            vm.CreatedAt = RealEstate.CreatedAt;
            vm.UpdatedAt = RealEstate.UpdatedAt;


            return View(vm);
        }
    }
}
