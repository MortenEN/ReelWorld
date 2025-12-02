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
        public async Task<IActionResult> Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return RedirectToAction("Index", "Event");

            var allEvents = await _eventApiClient.GetAllAsync();

            var filtered = allEvents
                .Where(e => e.Title.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                            (e.Description != null && e.Description.Contains(query, StringComparison.OrdinalIgnoreCase)))
                .ToList();

            return View("~/Views/Event/Search.cshtml", filtered);
        }

    }

}
