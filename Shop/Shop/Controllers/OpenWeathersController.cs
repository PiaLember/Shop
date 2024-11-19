using Microsoft.AspNetCore.Mvc;

namespace Shop.Controllers
{
    public class OpenWeathersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
