using Microsoft.AspNetCore.Mvc;
using ReelWorld.ApiClient;
using ReelWorld.DataAccessLibrary.Interfaces;

namespace ReelWorld.Website.Controllers
{
    public class HomeController : Controller
    {
        IEventDaoAsync _eventApiClient = new EventApiClient("https://LocalHost:7204");

        public async Task<IActionResult> Index()
        {
            var events = await _eventApiClient.Get10LatestAsync();
            return View(events);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
