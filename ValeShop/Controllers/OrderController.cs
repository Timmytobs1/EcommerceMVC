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
using CloudinaryDotNet.Core;
using Mysqlx.Crud;
using System.Text;
using ValeShop.PaymentServices;
using Microsoft.Extensions.Configuration;
using RestSharp;

namespace ValeShop.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;
        private readonly PaystackService _paystackService;


        public  OrderController(ApplicationDbContext context, IEmailService emailService, PaystackService paystackService)
        {
            _context = context;
            _emailService = emailService;
            _paystackService = paystackService;
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
        public async Task<IActionResult> Store(BillingDetailsViewModel model)
        {
            if (!ModelState.IsValid)
            {

                return View("BillingDetails");
            }  

            var billingDetails = new BillingDetails
            {
                Id = Guid.NewGuid(),
                UserId = Guid.Parse(HttpContext.Session.GetString("userId")),
                CompanyName = model.CompanyName,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
                City = model.City,
                Country = model.Country,
                State = model.State,
                // StateId = model.StateId,
                IsActive = true
            };

            await _context.BillingDetails.AddAsync(billingDetails);
            await _context.SaveChangesAsync();
            var sessionId = HttpContext.Session.GetString("sessionId");
            var cartItems = _context.Carts
                .Include(c => c.Product)
                .Where(c => c.SessionId == sessionId)
                .Select(c => new CartViewModel()
                {
                    Name = c.Product.Name ?? "",
                    ImageUrl = c.Product.ImagePath ?? "",
                    Quantity = c.Quantity,
                    ProductId = c.ProductId,
                    Price = c.Product.Price,
                    Total = c.Product.Price * c.Quantity
                }).ToList();

            var cartItemViewModel = new CartItemViewModel
            {
                CartItems = cartItems

            };

            var userId = HttpContext.Session.GetString("userId");
            var sessionIda = HttpContext.Session.GetString("sessionId");
            var orders = new ValeShop.Models.Entities.Orders
            {
                UserId = Guid.Parse(userId),
                AmountPaid = cartItemViewModel.Total,
                PaymentDate = DateTime.Now,
                Status = 0,
                DeliveryDate = DateTime.Now
            };
            await _context.Orders.AddAsync(orders);

            var cartItem = _context.Carts.Where(c => c.SessionId == sessionIda).Include(x => x.Product).ToList();
            foreach (var item in cartItem)
            {
                var details = new OrderDetails
                {
                    Quantity = item.Quantity,
                    Price = item.Product.Price * item.Quantity,
                    UnitPrice = item.Product.Price,
                    ProductId = item.ProductId,
                    OrderId = orders.Id,
                    BillingDetailsId = billingDetails.Id
                };
                _context.OrderDetails.Add(details);
            }
            await _context.SaveChangesAsync();

            //after saving orders, we then retrieve the customer's order details

            var orderDetails = await _context.OrderDetails
            .Where(od => od.OrderId == orders.Id)
            .Include(od => od.Product)
            .ToListAsync();


            string orderDetailsHtml = FormatOrderDetailsForEmail(orders, orderDetails);



            TempData["BillingDetailsSuccess"] = "Order placed successfully";
            var user = _context.Users.FirstOrDefault(x => x.Id == Guid.Parse(userId));


            string subject = "Order Confirmation - Order #" + orders.Id;
            string body = $"Dear {user?.FirstName},<br><br>You have successfully placed an order. Below are your order details:<br><br>{orderDetailsHtml}<br><br>Best regards,<br>Ecommerce Store Team";
            await _emailService.SendEmailAsync(user.Email, subject, body);

            // Convert total amount to kobo
            var totalAmountInKobo = (int)(cartItemViewModel.CartItems.Sum(x => x.Total) * 100);

            try
            {
                var paymentResponse = await _paystackService.InitializePayment(totalAmountInKobo, user.Email);
                var jsonResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(paymentResponse);

                var paymentUrl = (string)jsonResponse.data?.authorization_url;



                if (string.IsNullOrEmpty(paymentUrl))
                {
                    throw new Exception("Failed to retrieve payment URL from Paystack.");
                }



                return Redirect(paymentUrl);
            }
            catch (Exception ex)
            {
                // Handle and log the exception
                return RedirectToAction("Error", new { message = ex.Message });
            }

          /*  return View("Success");*/
        }

        private string FormatOrderDetailsForEmail(Orders order, List<OrderDetails> orderDetails)
        {

            var sb = new StringBuilder();

            sb.Append("<h2>Order Details</h2>");
            sb.Append($"<p>Order Number: {order.Id}</p>");
            sb.Append($"<p>Order Date: {order.PaymentDate.ToString("MM/dd/yyyy")}</p>");
            sb.Append("<table border='1' cellpadding='5' cellspacing='0'>");
            sb.Append("<tr><th>Product Image</th><th>Product Name</th><th>Quantity</th><th>Unit Price</th><th>Total Price</th></tr>");
            decimal totals = 0;
            foreach (var item in orderDetails)
            {
                Console.WriteLine($"Product: {item.Product?.Name}, ImageUrl: {item.Product?.ImagePath}, Price: {item.Price}, Quantity: {item.Quantity}");

                sb.Append("<tr>");

                sb.Append($"<td><img src='{item.Product?.ImagePath}' alt='{item.Product?.Name}' style='width:100px; height:auto;' /></td>");

                sb.Append($"<td>{item.Product?.Name}</td>");

                sb.Append($"<td>{item.Quantity}</td>");

                sb.Append($"<td>{item.UnitPrice:C}</td>");

                sb.Append($"<td>{item.Price:C}</td>");

                sb.Append("</tr>");
                totals += item.Price;
            }

            sb.Append("</table>");
            sb.Append($"<p>Total Amount Paid: {totals:C}</p>");

            return sb.ToString();
        }

        /* [HttpPost]
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
         }*/
    }
}
