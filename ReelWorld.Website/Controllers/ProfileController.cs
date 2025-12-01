using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
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

        //TODO: Find den rigtige Uri
        IProfileDaoAsync _userApiClient = new ProfileApiClient("https://LocalHost:7204");
        // GET: ProfileController
        public ActionResult Index()
        {

            return View();
        }

        // GET: ProfileController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var profile = await _userApiClient.GetOneAsync(id);

            return View(profile);
        }


        // GET: ProfileController/Create
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
        public async Task<IActionResult> Edit(int id)
        {
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Profile profile, string[] Interests)
        {
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

            TempData["SuccessMessage"] = "Din profil blev opdateret!";
            return RedirectToAction("Details", new { id = profile.ProfileId });
        }

        // POST: ProfileController/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, IFormCollection collection)
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
