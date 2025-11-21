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
        public async Task<IActionResult> Create(Event @event, Profile profile)
        {
            Registration registration = new Registration(@event, profile);
            if (!ModelState.IsValid)
                return View(registration);

            await _registrationApiClient.JoinEventAsync(@event.EventId, profile.ProfileId);

            TempData["SuccessMessage"] = "Eventet blev oprettet!";
            return RedirectToAction("Index", "Home");
        }
    }
}
