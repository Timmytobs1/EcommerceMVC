using CloudinaryDotNet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ValeShop.Data;
using ValeShop.Interface;
using ValeShop.Models;

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
            if (HttpContext.Session.GetString("sessionId") == null)
            {
                 return RedirectToAction("Login", "User");
            }
            var products = _context.Products.Include(x => x.Category).ToList();
            return View(products);
        }
        [HttpGet]
        public IActionResult SingleProduct(Guid Id)
        {
            var singleProduct = _context.Products.FirstOrDefault(x => x.Id == Id);
            if (singleProduct == null)
            {
                return RedirectToAction("Shop", "Shop");
            }
            var image = singleProduct.ImagePath;
            ViewBag.ImageUrl = image;
            return View(singleProduct);
        }
        public async Task <IActionResult> ShoppingCart()
        {
            var sessionId = HttpContext.Session.GetString("sessionId");
            var cartItems = await _context.Carts
                .Include(c => c.Product)
                .Where(c => c.SessionId == sessionId)
                .Select(c => new CartViewModel
                {
                    Id = c.Id,
                    Name = c.Product.Name ?? "",
                    ImageUrl = c.Product.ImagePath ?? "",
                    Quantity = c.Quantity,
                    Price = c.Product.Price ,
                    Total = c.Product.Price * c.Quantity
                }).ToListAsync();

            return View(cartItems);
        }   
    }
}
