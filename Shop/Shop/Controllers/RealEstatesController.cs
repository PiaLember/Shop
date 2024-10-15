using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.ApplicationServices.Services;
using Shop.Core.Dto;
using Shop.Core.ServiceInterface;
using Shop.Data;
using Shop.Models.RealEstates;
using static System.Net.Mime.MediaTypeNames;

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
                Files = vm.Files,
                Image = vm.Image
                    .Select(x => new FileToDatabaseDto
                    {
                        Id = x.ImageId,
                        ImageData = x.ImageData,
                        ImageTitle = x.ImageTitle,
                        RealEstateId = x.RealEstateId,
                    }).ToArray()

            };

            var result = await _realEstatesServices.Create(dto);

            if (result == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index), vm);
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var realestate = await _realEstatesServices.DetailsAsync(id);

            if (realestate == null)
            {
                return NotFound();
            }
            var images = await _context.FileToDatabases
               .Where(x => x.RealEstateId == id)
               .Select(y => new RealEstateImageViewModel
               {
                   RealEstateId = y.Id,
                   ImageId = (Guid)y.Id,
                   ImageData = y.ImageData,
                   ImageTitle = y.ImageTitle,
                   Image = string.Format("data:image/gif;base64,{0}", Convert.ToBase64String(y.ImageData))
               }).ToArrayAsync();

            var vm = new RealEstateCreateUpdateViewModel();

            vm.Id = realestate.Id;
            vm.Location = realestate.Location;
            vm.Size = realestate.Size;
            vm.RoomNumber = realestate.RoomNumber;
            vm.BuildingType = realestate.BuildingType;
            vm.CreatedAt = realestate.CreatedAt;
            vm.UpdatedAt = realestate.UpdatedAt;
            vm.Image.AddRange(images);



            return View("CreateUpdate", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Update(RealEstateCreateUpdateViewModel vm)
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
                Files = vm.Files,
                Image = vm.Image
                    .Select(x => new FileToDatabaseDto
                    {
                        Id = x.ImageId,
                        ImageData = x.ImageData,
                        ImageTitle = x.ImageTitle,
                        RealEstateId = x.RealEstateId,
                    }).ToArray()

            };

            var result = await _realEstatesServices.Update(dto);

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
            var images = await _context.FileToDatabases
                .Where(x => x.RealEstateId == id)
                .Select(y => new RealEstateImageViewModel
                {
                    RealEstateId = y.Id,
                    ImageId = (Guid)y.Id,
                    ImageData = y.ImageData,
                    ImageTitle = y.ImageTitle,
                    Image = string.Format("data:image/gif;base64,{0}", Convert.ToBase64String(y.ImageData))
                }).ToArrayAsync();

            var vm = new RealEstateDetailsViewModel();

            vm.Id = RealEstate.Id;
            vm.Location = RealEstate.Location;
            vm.Size = RealEstate.Size;
            vm.RoomNumber = RealEstate.RoomNumber;
            vm.BuildingType = RealEstate.BuildingType;
            vm.CreatedAt = RealEstate.CreatedAt;
            vm.UpdatedAt = RealEstate.UpdatedAt;
            vm.Image.AddRange(images);


            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var RealEstate = await _realEstatesServices.DetailsAsync(id);

            if (RealEstate == null)
            {
                return NotFound();
            }
            var images = await _context.FileToDatabases
              .Where(x => x.RealEstateId == id)
              .Select(y => new RealEstateImageViewModel
              {
                  RealEstateId = y.Id,
                  ImageId = (Guid)y.Id,
                  ImageData = y.ImageData,
                  ImageTitle = y.ImageTitle,
                  Image = string.Format("data:image/gif;base64,{0}", Convert.ToBase64String(y.ImageData))
              }).ToArrayAsync();

            var vm = new RealEstateDeleteViewModel();

            vm.Id = RealEstate.Id;
            vm.Location = RealEstate.Location;
            vm.Size = RealEstate.Size;
            vm.RoomNumber = RealEstate.RoomNumber;
            vm.BuildingType = RealEstate.BuildingType;
            vm.CreatedAt = RealEstate.CreatedAt;
            vm.UpdatedAt = RealEstate.UpdatedAt;
            vm.Image.AddRange(images);


            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmation(Guid id)
        {
            var RealEstateId = await _realEstatesServices.Delete(id);

            if (RealEstateId == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveImage(RealEstateImageViewModel vm)
        {
            var dto = new FileToDatabaseDto()
            {
                Id = vm.ImageId
            };

            var image = await _fileServices.RemoveFileFromDatabase(dto);

            if (image == null)
            {
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }


    }
}
