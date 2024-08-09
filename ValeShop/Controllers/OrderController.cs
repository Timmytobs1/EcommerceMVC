using Microsoft.AspNetCore.Mvc;
using ValeShop.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ValeShop.Data;
using ValeShop.Interface;
using ValeShop.Models.Entities;

namespace ValeShop.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;
       
        public  OrderController(ApplicationDbContext context)
        {
            _context = context;
           
        }

        [HttpGet]
        public IActionResult CheckOut()
        {
            var sessionId = HttpContext.Session.GetString("sessionId");
            var cartItems = _context.Carts
                .Include(c => c.Product)
                .Where(c => c.SessionId == sessionId)
                .Select(c => new CartViewModel
                {
                    Id = c.Id,
                    Name = c.Product.Name ?? "",
                    ImageUrl = c.Product.ImagePath ?? "",
                    Quantity = c.Quantity,
                    Price = c.Product.Price,
                    Total = c.Product.Price * c.Quantity
                }).ToList();

            return View(cartItems);
        }

        [HttpGet]
        public IActionResult BillingDetails(BillingDetailsViewModel billingDetailsViewModel)
        {
            return View();
        }
        [HttpPost]
        public IActionResult store(BillingDetailsViewModel billingDetailsViewModel)
        {

         
            var billingmodel = new BillingDetails
            {

                UserId =  Guid.Parse(HttpContext.Session.GetString("userId")),
                CompanyName = billingDetailsViewModel.CompanyName,
                City = billingDetailsViewModel.City,
                Address = billingDetailsViewModel.Address,
                PhoneNumber = billingDetailsViewModel.PhoneNumber,
                State = billingDetailsViewModel.State,
                Country = billingDetailsViewModel.Country,
                IsActive = true
            };
            _context.BillingDetails.Add(billingmodel);
            _context.SaveChanges();
            return Ok("Successful");
        }


        public IActionResult CustomMyOrder()
        {
            return View();
        }
    }
}
