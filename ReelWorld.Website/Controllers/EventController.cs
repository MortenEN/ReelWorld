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

     
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> Details([FromRoute] int id)
        {
            var @event = await _eventApiClient.GetOneAsync(id);
            if (@event == null) 
            { 
                return NotFound(); 
            }
            return View(@event);
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
        public async Task<IActionResult> Create(Event @event)
        {
            if (!ModelState.IsValid)
                return View(@event);

            await _eventApiClient.CreateAsync(@event);

            TempData["SuccessMessage"] = "Eventet blev oprettet!";
            return RedirectToAction("Index", "Home");
        }

        // GET: EventController/Edit/5
        public ActionResult Edit(int eventid)
        {
            return View();
        }

        // POST: EventController/Edit/5
        [HttpPost]
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