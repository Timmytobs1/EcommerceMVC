using Microsoft.AspNetCore.Mvc;
using ValeShop.Data;
using ValeShop.Models.Entities;
using ValeShop.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ValeShop.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Add item to cart
        [HttpPost]
        public async Task<IActionResult> AddToCart(Guid id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            var sessionId = HttpContext.Session.GetString("sessionId");
            
            var userId = GetUserId(); // Implement this method based on your user authentication setup

            var cartItem = await _context.Carts.FirstOrDefaultAsync(x => x.ProductId == id && x.SessionId == sessionId);
         //  var cartItem = await _context.Carts.FirstOrDefaultAsync(x => x.ProductId == id);

            if (cartItem != null)
            {
                cartItem.Quantity++;
            }
            else
            {    
                cartItem = new Carts
                {
                    Id = Guid.NewGuid(),
                    ProductId = id,
                    SessionId = sessionId,
                    Quantity = 1,
                    UserId = new Guid(HttpContext.Session.GetString("userId")),
                  //     UserId = new Guid("fb0e49ef-c998-413e-bf07-767da941d6a8"),
                    CreatedAt = DateTime.Now
                };
                _context.Carts.Add(cartItem);
            }

            await _context.SaveChangesAsync();
            // return RedirectToAction(nameof(Cart));
            return RedirectToAction("Shop", "Shop");
               
        }

   

        // Display cart contents
        public IActionResult Cart()
        {
            var sessionId = HttpContext.Session.Id;
            var cartItems = _context.Carts
                .Where(b => b.SessionId == sessionId)
                .ToList();

            return View(cartItems);
        }

        // Update item quantity in cart
        [HttpPost]
        public async Task<IActionResult> UpdateCart(Guid cartItemId, int quantity)
        {
           
            var cartItem = await _context.Carts.FindAsync(cartItemId);
            if (cartItem == null)
            {
                return NotFound();
            }

            cartItem.Quantity = quantity;
            Console.WriteLine(quantity);
            await _context.SaveChangesAsync();

            return RedirectToAction("ShoppingCart", "Shop");
        }

        //  Romove item from cart
          [HttpPost]
           public async Task<IActionResult> RemoveFromCart(Guid cartItemId)
          {
           var cartItem = await _context.Carts.FindAsync(cartItemId);
          if (cartItem == null)
            {
             return NotFound();
            }

           _context.Carts.Remove(cartItem);
           await _context.SaveChangesAsync();

            return RedirectToAction("ShoppingCart", "Shop");
         }

        // Helper method to get user ID
        private Guid GetUserId()
        {
            // Implement logic to get the user's ID, for example, from the user claims or authentication service
            return Guid.NewGuid(); // Replace with actual user ID retrieval
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
