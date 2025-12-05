using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ReelWorld.ApiClient;
using ReelWorld.DataAccessLibrary.Interfaces;
using ReelWorld.DataAccessLibrary.Model;

namespace ReelWorld.Website.Controllers
{
    public class EventController : Controller
    {
        IEventDaoAsync _eventApiClient = new EventApiClient("https://LocalHost:7204");


        public async Task<ActionResult> Index()
        {
            var events = await _eventApiClient.Get10BiggestAsync();
            return View(events);
        }

        [HttpGet]
        public async Task<ActionResult> Details([FromRoute]int id)
        {
            var @event = await _eventApiClient.GetOneAsync(id);
            if (@event == null) 
            { 
                return NotFound(); 
            }
            return View(@event);
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.EventTypeList = new List<SelectListItem>
            {
            new SelectListItem { Value = "true", Text = "Offentlig Event" },
            new SelectListItem { Value = "false", Text = "Privat Event" }};

            return View();
        }

        // POST: EventController/Create
        [HttpPost]
        public async Task<IActionResult> Create(Event @event)
        {
            if (!ModelState.IsValid)
            {
                return View(@event);
            }

            await _eventApiClient.CreateAsync(@event);

            TempData["SuccessMessage"] = "Eventet blev oprettet!";
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var @event = await _eventApiClient.GetOneAsync(id);

            if (@event == null)
                return NotFound();

            ViewBag.EventTypeList = new List<SelectListItem>
            {
                new SelectListItem { Value = "true", Text = "Offentlig Event" },
                new SelectListItem { Value = "false", Text = "Privat Event" }
            };

            return View(@event);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Event @event)
        {
            if (id != @event.EventId)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(@event);

            await _eventApiClient.UpdateAsync(@event);

            TempData["SuccessMessage"] = "Eventet blev opdateret!";
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var deletedEvent = await _eventApiClient.DeleteAsync(id);

            if (deletedEvent == null)
                return NotFound();

            TempData["SuccessMessage"] = "Eventet blev slettet!";
            return RedirectToAction("Index", "Home");
        }

        [HttpGet] public async Task<IActionResult> SearchAsync(string query) 

        { 
            if (string.IsNullOrWhiteSpace(query)) return RedirectToAction("Index"); 
            
            var allEvents = await _eventApiClient.GetAllAsync(); 
            var filtered = allEvents.Where(e => e.Title.Contains(query, StringComparison.OrdinalIgnoreCase) 
            || (e.Description != null && e.Description.Contains(query, StringComparison.OrdinalIgnoreCase))).ToList(); 
            
            return View("Search", filtered); 
        }

    }
}