using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ReelWorld.ApiClient;
using ReelWorld.DataAccessLibrary.Interfaces;
using ReelWorld.DataAccessLibrary.Model;
using ReelWorld.DataAccessLibrary.SqlServer;
using System.Security.Claims;

namespace ReelWorld.Website.Controllers
{
    public class ProfileController : Controller
    {
        IProfileDaoAsync _userApiClient = new ProfileApiClient("https://LocalHost:7204");
        // GET: ProfileController
        public ActionResult Index()
        {

            return View();
        }

        // GET: ProfileController/Details/5
        [HttpGet]
        [Authorize]
        public async Task<ActionResult> Details(int id)
        {
            var loggedInUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (loggedInUserId != id)
                return Forbid();
            var profile = await _userApiClient.GetOneAsync(id);

            return View(profile);
        }

        // GET: ProfileController/Create
        [HttpGet]
        public ActionResult Create()
        {
            // Generer SelectList fra enum
            ViewBag.RelationshipList = Enum.GetValues(typeof(Profile.RelationshipStatus))
                                           .Cast<Profile.RelationshipStatus>()
                                           .Select(r => new SelectListItem
                                           {
                                               Value = r.ToString(),
                                               Text = r.ToString()
                                           }).ToList();
            return View();
        }

        // POST: ProfileController/Create
        [HttpPost]
        public async Task<IActionResult> Create(Profile profile, string[] Interests)
        {
            if (!ModelState.IsValid)
                return View(profile);

            profile.Interests = Interests != null ? string.Join(",", Interests) : "";

            await _userApiClient.CreateAsync(profile);

            TempData["SuccessMessage"] = "Bruger blev oprettet!";
            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public async Task<IActionResult> Profiles()
        {
            var profiles = await _userApiClient.GetAllAsync();
            return View(profiles);
        }

        [HttpGet]
        public async Task<ActionResult> Profile([FromRoute] int id)
        {
            var profile = await _userApiClient.GetOneAsync(id);
            if (profile == null)
            {
                return NotFound();
            }
            return View(profile);
        }



        // GET: ProfileController/Edit/5
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var loggedInUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (loggedInUserId != id)
                return Forbid();
            var profile = await _userApiClient.GetOneAsync(id);
            if (profile == null) return NotFound();

            // Optional: dropdowns
            ViewBag.RelationshipList = Enum.GetValues(typeof(Profile.RelationshipStatus))
                                           .Cast<Profile.RelationshipStatus>()
                                           .Select(r => new SelectListItem
                                           {
                                               Value = r.ToString(),
                                               Text = r.ToString()
                                           })
                                           .ToList();

            return View(profile);
        }

        // POST: ProfileController/Edit/5
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Profile profile, string[] Interests)
        {
            var loggedInUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (loggedInUserId != id)
                return Forbid();
            ViewBag.RelationshipList = Enum.GetValues(typeof(Profile.RelationshipStatus))
                                           .Cast<Profile.RelationshipStatus>()
                                           .Select(r => new SelectListItem
                                           {
                                               Value = r.ToString(),
                                               Text = r.ToString()
                                           })
                                           .ToList();
            if (id != profile.ProfileId)
                return BadRequest();

            if (!ModelState.IsValid)
            {
                return View(profile);
            }
            profile.Interests = Interests != null ? string.Join(", ", Interests) : "";

            var success = await _userApiClient.UpdateAsync(profile);
            if (!success) return NotFound();

            TempData["SuccessMessage"] = "Din profil blev opdateret korrekt!";
            return RedirectToAction("Details", new { id = profile.ProfileId });
        }

        // POST: ProfileController/Delete/5
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Delete(int id)
        {
            var loggedInUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (loggedInUserId != id)
                return Forbid();
            var success = await _userApiClient.DeleteAsync(id);
            if (!success)
                return NotFound();

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity());

            TempData["SuccessMessage"] = "Profilen blev slettet!";
            return RedirectToAction("Index", "Home");
        }
    }
}
