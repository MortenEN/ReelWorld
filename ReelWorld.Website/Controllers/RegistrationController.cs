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
        public async Task<IActionResult> Create(int eventId)
        {
            int profileId = 0;
            Registration registration = new Registration(eventId, profileId);
            if (!ModelState.IsValid)
                return View(registration);

            await _registrationApiClient.JoinEventAsync(eventId, eventId);

            TempData["SuccessMessage"] = "Eventet blev oprettet!";
            return RedirectToAction("Index", "Home");
        }
    }
}
