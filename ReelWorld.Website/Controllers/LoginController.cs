using Microsoft.AspNetCore.Mvc;
using ReelWorld.ApiClient;

namespace ReelWorld.Website.Controllers
{
    public class LoginController : Controller
    {
        private readonly LoginApiClient _LoginApiClient;

        public LoginController()
        {
            _LoginApiClient = new LoginApiClient("https://localhost:7204/");
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string email, string password)
        {
            int userId = await _LoginApiClient.LoginAsync(email, password);

            if (userId <= 0)
            {
                ViewBag.Error = "Forkert email eller adgangskode";
                return View();
            }

            // Gem login i session
            HttpContext.Session.SetInt32("UserId", userId);
            HttpContext.Session.SetString("UserEmail", email);

            return RedirectToAction("Index", "Home");
        }
    }
}

