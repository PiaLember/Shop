using Microsoft.AspNetCore.Mvc;

namespace Shop.Models.OpenWeathers
{
    public class IndexViewModel : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
