using Microsoft.AspNetCore.Mvc;
using ReelWorld.ApiClient;
using ReelWorld.DataAccessLibrary.Interfaces;
using System.Security.Claims;

namespace ReelWorld.Website.Controllers
{
    public class HomeController : Controller
    {
        IEventDaoAsync _eventApiClient = new EventApiClient("https://LocalHost:7204");

        public async Task<IActionResult> Index(string sort = "biggest")
        {
            var events = await _eventApiClient.Get10BiggestAsync();

            if (sort == "latest")
            {
                events = await _eventApiClient.Get10LatestAsync();
            }
            else if (sort == "biggest")
            {
                events = events.OrderByDescending(e => e.AttendeeCount).ToList();
            }

            ViewData["Sort"] = sort;
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
