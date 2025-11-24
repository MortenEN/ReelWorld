using Microsoft.AspNetCore.Mvc;
using ReelWorld.ApiClient;
using ReelWorld.DataAccessLibrary.Interfaces;
using ReelWorld.DataAccessLibrary.Model;

namespace ReelWorld.Website.Controllers
{
    public class RegistrationController : Controller
    {
        IRegistrationDaoAsync _registrationApiClient = new RegistrationApiClient("https://LocalHost:7204");

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] int eventId, int profileId)
        {
            Registration registration = new Registration(eventId, profileId);
            if (!ModelState.IsValid)
                return View(registration);

            await _registrationApiClient.JoinEventAsync(eventId, profileId);

            TempData["SuccessMessage"] = "Du er nu tilmeldt eventet";
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public async Task<IActionResult> Create(int id)
        {
            Registration registration = new() { EventId = id };
            return View(registration);
        }
    }
}
