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
        IEventDao _eventApiClient = new EventApiClient("https://LocalHost:7221");

     
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Details(int eventid)
        {
            return View();
        }

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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Event @event)
        {
            //TODO: En slags besked om at oprettelsen er lykkedes
            if (!ModelState.IsValid)
                return View(@event);

            await _eventApiClient.CreateEventAsync(@event);

            return RedirectToAction("Index");
        }

        // GET: EventController/Edit/5
        public ActionResult Edit(int eventid)
        {
            return View();
        }

        // POST: EventController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int eventid, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: EventController/Delete/5
        public ActionResult Delete(int eventid)
        {
            return View();
        }

        // POST: EventController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int eventid, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}