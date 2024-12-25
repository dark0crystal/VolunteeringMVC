using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using volunteeringMVC.Models;
using System.Linq;

namespace volunteeringMVC.Controllers
{
    public class AuthController : Controller
    {
        

        MasterContext context = new MasterContext();

        // Login
        public IActionResult Login()
        {
            return View();
        }
        public Register user;
        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            if (ModelState.IsValid)
            {
                this.user = context.Registers.FirstOrDefault(u => u.Email == email && u.Password == password);

            }

            if (user != null)
            {
                // Set session
                HttpContext.Session.SetString("UserId", user.Id.ToString());
                HttpContext.Session.SetString("UserName", user.FirstName);
                HttpContext.Session.SetString("UserEmail", user.Email);

                // Set cookie
                Response.Cookies.Append("UserEmail", user.Email, new CookieOptions { Expires = DateTime.Now.AddHours(1) });
               
                return RedirectToAction("Index", "Home");
            }
            
            return View();
        }

        // Register
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(Register model)
        {
            if (ModelState.IsValid)
            {
                // Save user to database
                context.Registers.Add(model);
                context.SaveChanges();

                // Redirect to login
                return RedirectToAction("Login");
            }
            return View(model);
        }

        // Logout
        public IActionResult Logout()
        {
            // Clear session
            HttpContext.Session.Clear();

            // Clear cookies
            Response.Cookies.Delete("UserEmail");

            return RedirectToAction("Login");
        }
    }
}
