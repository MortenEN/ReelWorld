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
            var allEvents = await _eventApiClient.GetAllAsync();

            var filtered = allEvents.AsQueryable();

            // Søgning
            if (!string.IsNullOrWhiteSpace(query))
            {
                filtered = filtered.Where(e =>
                    e.Title.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                    (e.Description != null && e.Description.Contains(query, StringComparison.OrdinalIgnoreCase))
                );
            }

            // Filter på kategori
            if (!string.IsNullOrWhiteSpace(category) && category != "All")
            {
                filtered = filtered.Where(e => e.Category == category);
            }

            return View("~/Views/Event/Search.cshtml", filtered.ToList());
        }


    }

}
