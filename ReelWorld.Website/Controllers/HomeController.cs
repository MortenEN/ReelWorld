using Microsoft.AspNetCore.Mvc;
using ReelWorld.Website.Models;
using System.Diagnostics;

namespace ReelWorld.Website.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;//???

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
