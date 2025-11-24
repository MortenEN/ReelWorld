using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ReelWorld.ApiClient;
using ReelWorld.DataAccessLibrary.Interfaces;
using ReelWorld.DataAccessLibrary.Model;
using ReelWorld.DataAccessLibrary.SqlServer;

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
        public ActionResult Details(int id)
        {
            var profile = _userApiClient.GetOneAsync(id);
            
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
        public async Task<IActionResult> Create(Profile profile)
        {
            if (!ModelState.IsValid)
                return View(profile);

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
        public async Task<ActionResult> profile([FromRoute] int id)
        {
            var profile = await _userApiClient.GetOneAsync(id);
            if ( profile == null)
            {
                return NotFound();
            }
            return View(profile);
        }


        // GET: ProfileController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ProfileController/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(Profile profile)
        {
            if (!ModelState.IsValid)
                return View(profile);

            await _userApiClient.UpdateAsync(profile);

            TempData["SuccessMessage"] = "Bruger blev opdateret!";
            return RedirectToAction("Index", "Profile");
        }

        // GET: ProfileController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
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
