using Microsoft.AspNetCore.Mvc;

namespace ValeShop.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
