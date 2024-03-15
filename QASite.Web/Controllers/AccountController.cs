using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using QASite.Data;
using System.Security.Claims;

namespace StackOverflow.Web.Controllers
{
    public class AccountController : Controller
    {
        private string _connectionString;
        public AccountController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConStr");
        }
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SignUp(User user, string password)
        {
            var repo = new UserRepository(_connectionString);
            repo.AddUser(user, password);
            return Redirect("/account/login");
        }
        public IActionResult Login()
        {
            if (TempData["Message"] != null)
            {
                ViewBag.Message = "Invalid Login!";
            }
            return View();
        }
        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var repo = new UserRepository(_connectionString);
            var user = repo.GetUserForLogin(email, password);
            if (user == null)
            {
                TempData["Message"] = "Invalid Login!";
                return Redirect("/account/login");
            }
            var claims = new List<Claim>
            {
                new Claim("user",email)
            };
            HttpContext.SignInAsync(new ClaimsPrincipal(new ClaimsIdentity(claims, "Cookies", "user", "role"))).Wait();
            return Redirect("/home/index");
        }
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync().Wait();
            return Redirect("/account/login");
        }
    }
}
