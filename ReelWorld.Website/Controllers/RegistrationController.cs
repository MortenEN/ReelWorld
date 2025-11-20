using Microsoft.AspNetCore.Mvc;

namespace ReelWorld.Website.Controllers
{
    public class RegistrationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
