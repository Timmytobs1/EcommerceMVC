using Microsoft.AspNetCore.Mvc;
using ValeShop.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ValeShop.Data;
using ValeShop.Interface;
using ValeShop.Models.Entities;
using System.Net.Mail;
using System.Net;

namespace ValeShop.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;


        public  OrderController(ApplicationDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
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
        public async Task<IActionResult> Store(BillingDetailsViewModel billingDetailsViewModel)
        {
            var userId = Guid.Parse(HttpContext.Session.GetString("userId"));

            // Retrieve user email from the database
            var user = _context.Users.FirstOrDefault(x => x.Id == userId);
            if (user == null || string.IsNullOrEmpty(user.Email))
            {
                // Handle the case where the user or their email is not found
                TempData["ErrorMessage"] = "Unable to retrieve user email. Please try again.";
                return View("Error");
            }

            var billingmodel = new BillingDetails
            {
                UserId = userId,
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
            var orderedProducts = _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .Where(o => o.UserId == userId)
                .SelectMany(o => o.OrderDetails)
                .Select(od => new
                {
                    od.Product.Name,
                    od.Quantity,
                    od.Price
                })
                .ToList();


            // Create a list of ordered products for the email
            // Generate the ordered products list
            string orderedProductsList = "<ul>";
            if (orderedProducts.Any())
            {
                foreach (var product in orderedProducts)
                {
                    orderedProductsList += $"<li>{product.Name} (Quantity: {product.Quantity}, Price: {product.Price:C})</li>";
                }
            }
            else
            {
                orderedProductsList += "<li>No products found.</li>";
            }
            orderedProductsList += "</ul>";

            // Send a confirmation email after saving billing details
            string subject = "Billing Details Confirmation";
            string message = $@"
<h1>Billing Details Received Successfully</h1>
<p>Dear {user.FirstName},</p>
<p>Thank you for providing your billing details. Here are the details you entered:</p>
<ul>
    <li><strong>Company Name:</strong> {billingDetailsViewModel.CompanyName}</li>
    <li><strong>City:</strong> {billingDetailsViewModel.City}</li>
    <li><strong>Address:</strong> {billingDetailsViewModel.Address}</li>
    <li><strong>Phone Number:</strong> {billingDetailsViewModel.PhoneNumber}</li>
    <li><strong>State:</strong> {billingDetailsViewModel.State}</li>
    <li><strong>Country:</strong> {billingDetailsViewModel.Country}</li>
    <li><strong>Zip:</strong> {billingDetailsViewModel.Zip}</li>
</ul>
<p>Here are the products you ordered:</p>
{orderedProductsList}
<p>If you need to make any changes, please visit your account to update your details.</p>
<br/>
<p>Best regards,</p>
<p>The Ecommerce Store Team</p>
";

            await _emailService.SendEmailAsync(user.Email, subject, message);


            return View("Success");
        }



        public IActionResult CustomMyOrder()
        {
            return View();
        }
    }
}
