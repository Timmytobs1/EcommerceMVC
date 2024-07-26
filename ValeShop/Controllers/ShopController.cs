using Microsoft.AspNetCore.Mvc;

namespace ValeShop.Controllers
{
    public class ShopController : Controller
    {
        public IActionResult Shop()
        {
            return View();
        }
        public IActionResult SingleProduct()
        {
            return View();
        }
        public IActionResult ShoppingCart()
        {
            return View();
        }
    }
}
