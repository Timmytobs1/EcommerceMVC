using Microsoft.AspNetCore.Mvc;

namespace ValeShop.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult CheckOut()
        {
            return View();
        }
        public IActionResult CustomMyOrder()
        {
            return View();
        }
    }
}
