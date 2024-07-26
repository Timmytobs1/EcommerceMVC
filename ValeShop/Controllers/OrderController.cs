using Microsoft.AspNetCore.Mvc;

namespace ValeShop.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult CheckOut()
        {
            return View();
        }
        public IActionResult CustomeMyOrder()
        {
            return View();
        }
    }
}
