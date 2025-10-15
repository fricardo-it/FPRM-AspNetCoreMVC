using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Linq;
using FPRMAspNetCoreMVC.Data;
using FPRMAspNetCoreMVC.ViewModels;
using FPRMAspNetCoreMVC.Models;
using System.Threading.Tasks;

namespace FPRMAspNetCoreMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var user = _context.User.FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password);

            if (user != null)
            {
                // Configure o usuário no contexto
                HttpContext.Items["User"] = user;

                var userClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Role, user.UserRole.ToString()) 
                };

                var userIdentity = new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);
                var userPrincipal = new ClaimsPrincipal(userIdentity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal);

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt");
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Signup([Bind("Name,Email,Password,Id")] User user)
        {
            if (ModelState.IsValid)
            {
                user.UserRole = UserRole.Tenant;
                _context.Add(user);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "User registered successfully!"; 
                return RedirectToAction("Login", "Account");
            }

            return View("Signup", user);
        }


    }
}
