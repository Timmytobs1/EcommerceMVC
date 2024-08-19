using System.ComponentModel;
using BCrypt.Net;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using MySql.Data.MySqlClient;
using UAParser;
using ValeShop.Data;
using ValeShop.Interface;
using ValeShop.Models;
using ValeShop.Models.Entities;

namespace ValeShop.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository repo;
        private readonly IEmailService _emailService;
        private readonly ApplicationDbContext _context;

        public UserController(IUserRepository repository, IEmailService emailService, ApplicationDbContext context)
        {
            _context = context;
            repo = repository;
            _emailService = emailService;
        }
        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("sessionId") != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Success()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(UserViewModel userViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                var user = await repo.CreateUser(userViewModel);

                if (string.IsNullOrEmpty(user.Email))
                {
                    ModelState.AddModelError(string.Empty, "Email address is required.");
                    return View(userViewModel);
                }

                string subject = "Welcome to ValeShop!";
                string message = $@"
    <h1>Welcome to Our Ecommerce Store!</h1>
   <p>Dear {userViewModel.FirstName},</p>
    <p>Thank you for registering with us. We're thrilled to have you as part of our community.</p>
    <p>You can now explore a wide range of products, enjoy exclusive offers, and make secure purchases.</p>
    <p>Don't forget to check out our latest arrivals and special deals tailored just for you.</p>
    <p>If you have any questions or need assistance, our customer support team is always here to help.</p>
    <p>Thank you for choosing us, and happy shopping!</p>
    <br/>
    <p>Best regards,</p>
    <p>The Ecommerce Store Team</p>
";

                await _emailService.SendEmailAsync(user.Email, subject, message);

                /*         var message = new MimeMessage();
                         message.From.Add(new MailboxAddress("Ecommerce", "ValeShop@gmail.com"));
                         message.To.Add(new MailboxAddress(" Test      test           test", user.Email));
                         *//*          message.Subject = "Login Notification";*//*
                         message.Subject = "Successful Registration";
                         message.Body = new TextPart("plain")
                         {
                             *//*
                                                 Text = $"You logged in at {DateTime.Now}"*//*

                             Text = $"You've successfully registered. Thank you for joining the Ecommerce store, you can proceed to shop with us"
                         };
                         using (var client = new SmtpClient())
                         {
                             client.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                             client.Authenticate("obadaratimothy@gmail.com", "nkholcupfyhfpmda");
                             client.Send(message);
                             client.Disconnect(true);
                         }*/
                TempData["SuccessMessage"] = "User successfully registered";
                return RedirectToAction(nameof(Success));
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is MySqlException e && (e.Number == 1062))
                {
                    if (e.Message.Contains("Email") && e.Message.Contains("UserName"))
                    {
                        return BadRequest("Email and UserName already exist.");
                    }
                    else if (e.Message.Contains("Email"))
                    {
                        return BadRequest("Email already exists.");
                    }
                    return BadRequest("UserName already exists.");
                }

                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel login)
        {
            if (HttpContext.Session.GetString("sessionId") != null)
            {
                return RedirectToAction("Index", "Home");
            }

            var templogin = _context.Users.FirstOrDefault(x => x.Email == login.Email);

            if (templogin == null)
            {
                TempData["ErrorMessage"] = "Invalid login details. Please try again.";
                return View("Login");
            }

            if (templogin != null && BCrypt.Net.BCrypt.Verify(login.Password, templogin.Password))
            {
                HttpContext.Session.SetString("sessionId", Guid.NewGuid().ToString());
                HttpContext.Session.SetString("userId", templogin.Id.ToString());

                // Capture the IP address
                string ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "IP Not Available";

                // Capture device information
                var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();
                var uaParser = Parser.GetDefault();
                var clientInfo = uaParser.Parse(userAgent);

                string device = $"{clientInfo.Device.Family} {clientInfo.OS.Family} {clientInfo.UA.Family}";

                string subject = "Login Notification";
                string message = $@"
        <h1>Login Notification</h1>
        <p>Dear Customer,</p>
        <p>This is to inform you that a login to your account was detected.</p>
        <p><strong>Login Details:</strong></p>
        <ul>
            <li><strong>Date and Time:</strong> {DateTime.Now}</li>

            <li><strong>Device:</strong> {device}</li>
        </ul>
        <p>If this was you, no further action is required.</p>
        <p>If you did not perform this login, please reset your password immediately and contact our support team.</p>
        <br/>
        <p>Best regards,</p>
        <p>The Ecommerce Store Team</p>
        ";

                await _emailService.SendEmailAsync(login.Email, subject, message);
                return RedirectToAction("Index", "Home");
            }

            TempData["ErrorMessage"] = "Invalid login details. Please try again.";
            return View("Login");
        }


        [HttpGet]
        public IActionResult Logout()
        {
            if (HttpContext.Session.GetString("sessionId") != null)
            {
                HttpContext.Session.Clear();

            }
            return RedirectToAction("Login", "User");
        }


    }
}

/*using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using ValeShop.Data;
using ValeShop.Interface;
using ValeShop.Models;
using System.Threading.Tasks;

public class UserController : Controller
{
    private readonly IUserRepository repo;
    private readonly IEmailService _emailService;
    private readonly ApplicationDbContext _context;

    public UserController(IUserRepository repository, IEmailService emailService, ApplicationDbContext context)
    {
        _context = context;
        repo = repository;
        _emailService = emailService;
    }

    [HttpGet]
    public IActionResult Success()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Login()
    {
        if (HttpContext.Session.GetString("sessionId") != null)
        {
            return RedirectToAction("Index", "Home");
        }
        return View();
    }
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(UserViewModel userViewModel)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }
        try
        {
            var user = await repo.CreateUser(userViewModel);
            TempData["SuccessMessage"] = "User successfully registered";

            // Send confirmation email
            string subject = "Welcome to ValeShop!";
            string message = $"Hi {user.FirstName},<br/><br/>Thank you for registering at ValeShop. We are glad to have you!";

            await _emailService.SendEmailAsync(user.Email, subject, message);

            return RedirectToAction(nameof(Success));
        }
        catch (DbUpdateException ex)
        {
            if (ex.InnerException is MySqlException e && (e.Number == 1062))
            {
                if (e.Message.Contains("Email") && e.Message.Contains("UserName"))
                {
                    return BadRequest("Email and UserName already exists");
                }
                else if (e.Message.Contains("Email"))
                {
                    return BadRequest("Email already exists");
                }
                return BadRequest("UserName already exist");
            }
            return View();
        }
    }

    [HttpPost]
    public IActionResult Login(LoginViewModel login)
    {
        if (HttpContext.Session.GetString("sessionId") != null)
        {
            return RedirectToAction("Index", "Home");
        }
        var templogin = _context.Users.FirstOrDefault(x => x.Email == login.Email);

        if (templogin == null)
        {
            TempData["ErrorMessage"] = "Invalid login details. Please try again.";
            return View("Login");
        }
        if (templogin != null && BCrypt.Net.BCrypt.Verify(login.Password, templogin.Password))
        {
            HttpContext.Session.SetString("sessionId", Guid.NewGuid().ToString());

            HttpContext.Session.SetString("userId", templogin.Id.ToString());
            return RedirectToAction("Index", "Home");
        }
        TempData["ErrorMessage"] = "Invalid login details. Please try again.";

        return View("Login");

    }

    [HttpGet]
    public IActionResult Logout()
    {
        if (HttpContext.Session.GetString("sessionId") != null)
        {
            HttpContext.Session.Clear();

        }
        return RedirectToAction("Login", "User");
    }

}
*/