using System.ComponentModel;
using BCrypt.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using ValeShop.Data;
using ValeShop.Interface;
using ValeShop.Models;

namespace ValeShop.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository repo;
        private readonly ApplicationDbContext _context;

        public UserController(IUserRepository repository, ApplicationDbContext context)
        {
            _context = context;
            repo = repository;

        }
   
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpGet]
        public  IActionResult Success()
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
                return RedirectToAction(nameof(Success)); // Redirect to the Index action or another action
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
            var templogin = _context.Users.FirstOrDefault(x => x.Email == login.Email || x.UserName == x.UserName);

            if (templogin == null)
            {
                TempData["ErrorMessage"] = "Invalid login details. Please try again.";
                return View("Login");
            }
            if(BCrypt.Net.BCrypt.Verify(login.Password, templogin.Password)){
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
}
