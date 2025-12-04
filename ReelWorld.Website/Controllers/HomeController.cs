using Microsoft.AspNetCore.Mvc;
using ReelWorld.ApiClient;
using ReelWorld.DataAccessLibrary.Interfaces;
using System.Security.Claims;

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

        [HttpGet]
        public async Task<IActionResult> Search(string query, string category)
        {
            var filteredEvents = await _eventApiClient.SearchAsync(query, category);

            ViewBag.Categories = (await _eventApiClient.GetAllAsync())
            .Select(e => e.Category).Distinct().ToList();

            return View("~/Views/Event/Search.cshtml", filteredEvents);
        }


    }

}
