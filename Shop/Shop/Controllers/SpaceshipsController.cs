using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.ApplicationServices.Services;
using Shop.Core.Domain;
using Shop.Core.Dto;
using Shop.Core.ServiceInterface;
using Shop.Data;
using Shop.Models.Spaceships;

namespace Shop.Controllers
{
    public class SpaceshipsController : Controller
    {

        private readonly ShopContext _context;
        private readonly ISpaceshipsServices _spaceshipsServices;
        private readonly IFileServices _fileServices;

        public SpaceshipsController
            (
                ShopContext context,
                ISpaceshipsServices spaceshipsServices,
                IFileServices fileServices

            )
        {
            _context = context;
            _spaceshipsServices = spaceshipsServices;
            _fileServices = fileServices;

        }


        public IActionResult Index()
        {
            var result = _context.Spaceships
                .Select(x => new SpaceshipsIndexViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Typename = x.Typename,
                    BuiltDate = x.BuiltDate,
                    Crew = x.Crew
                });

            return View(result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            SpaceshipCreateUpdateViewModel result = new();
            return View("CreateUpdate", result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(SpaceshipCreateUpdateViewModel vm)
        {
            var dto = new SpaceshipDto()
            {
                Id = vm.Id,
                Name = vm.Name,
                Typename = vm.Typename,
                SpaceshipModel = vm.SpaceshipModel,
                //BuiltDate = (DateTime)vm.BuiltDate,
                Crew = vm.Crew,
                EnginePower = vm.EnginePower,
                CreatedAt = vm.CreatedAt,
                ModifiedAt = vm.ModifiedAt,
                Files = vm.Files,
                FileToApiDtos = vm.Image
                    .Select(x => new FileToApiDto
                        {
                            Id = x.ImageId,
                            ExistingFilePath = x.FilePath,
                            SpaceshipId = x.SpaceshipId
                         }).ToArray()
            };
            var result = await _spaceshipsServices.Create(dto);
            if (result == null)
            {
                return BadRequest(new { success = false, message = "Failed to create real estate." });
            }

            if (Request.Headers["Accept"].ToString().Contains("application/json"))
            {
                return Ok(new { success = true, id = result.Id }); // JSON response
            }

            return RedirectToAction(nameof(Index)); // Redirect for web clients
        }



        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var spaceship = await _spaceshipsServices.DetailAsync(id);

            if (spaceship == null)
            {
                return View("Error");
            }
            var images = await _context.FileToApis
                .Where(x => x.SpaceshipId == id)
                .Select(y => new ImageViewModel
                {
                    FilePath = y.ExistingFilePath,
                    ImageId = y.Id
                }).ToArrayAsync();

            var vm = new SpaceshipDetailsViewModel();
            vm.Id = spaceship.Id;
            vm.Name = spaceship.Name;
            vm.Typename = spaceship.Typename;
            vm.SpaceshipModel = spaceship.SpaceshipModel;
            vm.BuiltDate = spaceship.BuiltDate;
            vm.Crew = spaceship.Crew;
            vm.EnginePower = spaceship.EnginePower;
            vm.CreatedAt = spaceship.CreatedAt;
            vm.ModifiedAt = spaceship.ModifiedAt;
            vm.Image.AddRange(images);

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var spaceship = await _spaceshipsServices.DetailAsync(id);

            if (spaceship == null)
            {
                return NotFound();
            }
            var images = await _context.FileToApis
                .Where(x => x.SpaceshipId == id)
                .Select(y => new ImageViewModel
                {
                    FilePath = y.ExistingFilePath,
                    ImageId = y.Id
                }).ToArrayAsync();

            var vm = new SpaceshipCreateUpdateViewModel();

            vm.Id = spaceship.Id;
            vm.Name = spaceship.Name;
            vm.Typename = spaceship.Typename;
            vm.SpaceshipModel = spaceship.SpaceshipModel;
            //vm.BuiltDate = spaceship.BuiltDate;
            vm.Crew = spaceship.Crew;
            vm.EnginePower = spaceship.EnginePower;
            vm.CreatedAt = spaceship.CreatedAt;
            vm.ModifiedAt = spaceship.ModifiedAt;
            vm.Image.AddRange(images);

            return View("CreateUpdate", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Update(SpaceshipCreateUpdateViewModel vm)
        {
            var dto = new SpaceshipDto()
            {
                Id = vm.Id,
                Name = vm.Name,
                Typename = vm.Typename,
                SpaceshipModel = vm.SpaceshipModel,
                //BuiltDate = (DateTime)vm.BuiltDate,
                Crew = vm.Crew,
                EnginePower = vm.EnginePower,
                CreatedAt = vm.CreatedAt,
                ModifiedAt = vm.ModifiedAt,
                Files = vm.Files,
                FileToApiDtos = vm.Image
                    .Select(x => new FileToApiDto
                    {
                        Id = x.ImageId,
                        ExistingFilePath = x.FilePath,
                        SpaceshipId = x.SpaceshipId,
                    }).ToArray()
            };

            var result = await _spaceshipsServices.Update(dto);

            if (result == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index), vm);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var spaceship = await _spaceshipsServices.DetailAsync(id);

            if (spaceship == null)
            {
                return NotFound();
            }
            var images = await _context.FileToApis
                .Where(x => x.SpaceshipId == id)
                .Select(y => new ImageViewModel
                {
                    FilePath = y.ExistingFilePath,
                    ImageId = y.Id
                }).ToArrayAsync();


            var vm = new SpaceshipDeleteViewModel();

            vm.Id = spaceship.Id;
            vm.Name = spaceship.Name;
            vm.Typename = spaceship.Typename;
            vm.SpaceshipModel = spaceship.SpaceshipModel;
            //vm.BuiltDate = spaceship.BuiltDate;
            vm.Crew = spaceship.Crew;
            vm.EnginePower = spaceship.EnginePower;
            vm.CreatedAt = spaceship.CreatedAt;
            vm.ModifiedAt = spaceship.ModifiedAt;
            vm.Image.AddRange(images);


            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmation(Guid id)
        {
            var spaceshipId = await _spaceshipsServices.Delete(id);

            if (spaceshipId == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveImage(ImageViewModel vm)
        {
            var dto = new FileToApiDto()
            {
                Id = vm.ImageId
            };

            var image = await _fileServices.RemoveImageFromApi(dto);

            if (image == null)
            {
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
