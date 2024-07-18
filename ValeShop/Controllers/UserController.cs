using Microsoft.AspNetCore.Mvc;

namespace ValeShop.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
