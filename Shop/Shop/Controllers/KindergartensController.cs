using Microsoft.AspNetCore.Mvc;
using Shop.Core.Dto;
using Shop.Core.ServiceInterface;
using Shop.Data;
using Shop.Models.Kindergartens;


namespace Shop.Controllers
{
    public class KindergartensController : Controller
    {
        private readonly ShopContext _context;
        private readonly IKindergartensServices _kindergartenServices;

        public KindergartensController
            (
                ShopContext context,
                IKindergartensServices kindergartensServices

            )
        {
            _context = context;
            _kindergartenServices = kindergartensServices;
        }
        public IActionResult Index()
        {
            var result = _context.Kindergartens
                .Select(x => new KindergartensIndexViewModel
                {
                    Id = x.Id,
                    KindergartenName = x.KindergartenName,
                    GroupName = x.GroupName,
                });
            return View(result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            KindergartenCreateUpdateViewModel result = new KindergartenCreateUpdateViewModel();
            return View("CreateUpdate", result);
        }
        [HttpPost]
        public async Task<IActionResult> Create(KindergartenCreateUpdateViewModel vm)
        {
            var dto = new KindergartenDto()
            {
                Id= vm.Id,
                KindergartenName= vm.KindergartenName,
                GroupName= vm.GroupName,
                ChildrenCount = vm.ChildrenCount,
                Teacher = vm.Teacher,
                CreatedAt = vm.CreatedAt,
                UpdatedAt = vm.UpdatedAt
            };
            var result = await _kindergartenServices.Create(dto);
            if (result == null)
            {
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index), vm);
        }
    }
}
