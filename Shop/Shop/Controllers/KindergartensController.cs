using Microsoft.AspNetCore.Mvc;
using Shop.Data;
using Shop.Models.Kindergartens;


namespace Shop.Controllers
{
    public class KindergartensController : Controller
    {
        private readonly ShopContext _context;

        public KindergartensController
            (ShopContext context)
        {
            _context = context;
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
    }
}
