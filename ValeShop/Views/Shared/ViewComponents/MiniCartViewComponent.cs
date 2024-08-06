using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ValeShop.Data;
using ValeShop.Models;

namespace ValeShop.Views.Shared.ViewComponents
{
    public class MiniCartViewComponent : ViewComponent
    {

        private readonly ApplicationDbContext _context;

        public MiniCartViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            //    var sessionId = HttpContext.Session.GetString("sessionId");
            var sessionId = HttpContext.Session.GetString("sessionId");
            var cartItems = await _context.Carts
                .Include(c => c.Product)
                .Where(c => c.SessionId == sessionId)
                .Select(c => new CartViewModel()
                {
                    Name = c.Product.Name ?? "",
                    ImageUrl = c.Product.ImagePath ?? "",
                    Quantity = c.Quantity,
                    Price = c.Product.Price,
                    Total = c.Product.Price * c.Quantity,
                }).ToListAsync();

            var cartItemViewModel = new CartItemViewModel()
            {
                CartItems = cartItems
            };

            return View(cartItemViewModel); // This will look for a view named Default.cshtml
        }
    }
}
