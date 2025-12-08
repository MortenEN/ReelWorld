using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReelWorld.ApiClient;
using ReelWorld.DataAccessLibrary.Interfaces;
using ReelWorld.DataAccessLibrary.Model;
using System.Security.Claims;

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
        [Authorize]
        public async Task<IActionResult> Join([FromForm] int eventId)
        {
            var profileId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (profileId == null)
                return Unauthorized();

            int loggedInUserId = int.Parse(profileId);

            Registration registration = new Registration(eventId, loggedInUserId);

            if (!ModelState.IsValid)
                return View(registration);

            bool joined = await _registrationApiClient.JoinEventAsync(eventId, loggedInUserId);

            if (!joined)
            {
                TempData["ErrorMessage"] = "Eventet er fuldt eller du er allerede tilmeldt.";
                return RedirectToAction("Index", "Home");
            }

            TempData["SuccessMessage"] = "Du er nu tilmeldt eventet.";
            return RedirectToAction("Index", "Home");
        }



        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Create(int id)
        {
            Registration registration = new() { EventId = id };
            return View(registration);
        }
    }
}
