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
            var events = await _eventApiClient.GetAllAsync();

            events = events.Where(e => e.Date >= DateTime.Now).ToList();

            var currentProfileId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var accessLevel = User.FindFirstValue(ClaimTypes.Role);
            bool isAdmin = accessLevel == "Admin";

            // Filtrer events baseret på synlighed
            var filteredEvents = events.Where(e =>
                e.IsPublic == true ||
                (currentProfileId != null && e.FK_Profile_Id.ToString() == currentProfileId) ||
                isAdmin
            ).ToList();

            if (sort == "latest")
            {
                filteredEvents = filteredEvents.OrderBy(e => e.Date).ToList(); // næste events først
            }
            else
            {
                filteredEvents = filteredEvents.OrderByDescending(e => e.AttendeeCount).ToList();
            }

            filteredEvents = filteredEvents.Take(10).ToList();

            ViewData["Sort"] = sort;
            return View(filteredEvents);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Search(string query)
        {
            var filteredEvents = await _eventApiClient.SearchAsync(query);

            var currentProfileId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var accessLevel = User.FindFirstValue(ClaimTypes.Role);
            bool isAdmin = accessLevel == "Admin";

            filteredEvents = filteredEvents.Where(e =>
                e.IsPublic == true ||
                (currentProfileId != null && e.FK_Profile_Id.ToString() == currentProfileId) ||
                isAdmin
            ).ToList();

            return View("~/Views/Event/Search.cshtml", filteredEvents);
        }


        [HttpGet]
        public async Task<IActionResult> SearchWithCategory(string query, string category)
        {
            var filteredEvents = await _eventApiClient.SearchWithCategoryAsync(query, category);
            
            var currentProfileId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var accessLevel = User.FindFirstValue(ClaimTypes.Role);
            bool isAdmin = accessLevel == "Admin";

            filteredEvents = filteredEvents.Where(e =>
                e.IsPublic == true ||
                (currentProfileId != null && e.FK_Profile_Id.ToString() == currentProfileId) ||
                isAdmin
            ).ToList();

            ViewBag.Categories = (await _eventApiClient.GetAllAsync())
                .Select(e => e.Category)
                .Distinct()
                .ToList();

            return View("~/Views/Event/Search.cshtml", filteredEvents);
        }



    }

}
