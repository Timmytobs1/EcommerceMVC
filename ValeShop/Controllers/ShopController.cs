using CloudinaryDotNet;
using Microsoft.AspNetCore.Mvc;
using ValeShop.Data;
using ValeShop.Interface;

namespace ValeShop.Controllers
{
    public class ShopController : Controller
    {
       
        private readonly ApplicationDbContext _context;

        public ShopController( ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Shop()
        {
            var products = _context.Products.ToList();
            return View(products);
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
