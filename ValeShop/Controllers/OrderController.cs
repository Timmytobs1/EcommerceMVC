using Microsoft.AspNetCore.Mvc;

namespace ValeShop.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
