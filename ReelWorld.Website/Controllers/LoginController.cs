using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using ReelWorld.ApiClient;
using ReelWorld.DataAccessLibrary.Model;
using ReelWorld.Website.Models;
using System.Security.Claims;

namespace ReelWorld.Website.Controllers
{
    public class LoginController : Controller
    {
        private readonly LoginApiClient _loginApiClient;

        public LoginController()
        {
            _loginApiClient = new LoginApiClient("https://localhost:7204");
        }

        public IActionResult Index()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            int userId;
            try
            {
                userId = await _loginApiClient.LoginAsync(model.Email, model.Password);
            }
            catch
            {
                ModelState.AddModelError("", "Fejl ved login. Tjek dine oplysninger.");
                return View(model);
            }

            if (userId <= 0)
            {
                ModelState.AddModelError("", "Forkert email eller adgangskode");
                return View(model);
            }
            var profile = await _loginApiClient.GetOneAsync(userId);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Email, model.Email),
                new Claim(ClaimTypes.Role, profile.AccessLevel.Name)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            TempData["Message"] = "You are now logged out.";
            return RedirectToAction("Index", "Home");
        }
    }
}
