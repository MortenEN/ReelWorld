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
        public async Task<IActionResult> Edit(int id, Profile profile)
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

        //[HttpPost]
        //public async Task<IActionResult> Login(Profile loginInfo, string? returnUrl)
        //{
        //    int profileId = await _userApiClient.LoginAsync(loginInfo.Email, loginInfo.HashPassword);

        //    if (profileId > 0)
        //    {
        //        var profile = await _userApiClient.GetOneAsync(profileId);

        //        var claims = new List<Claim>
        //{
        //    new Claim("profile_id", profile.ProfileId.ToString()),
        //    new Claim(ClaimTypes.Email, profile.Email),
        //    new Claim(ClaimTypes.Role, "User")
        //};

        //        await SignInUsingClaims(claims); // hvis du har samme helper som i BlogSharp

        //        HttpContext.Session.SetInt32("ProfileId", profile.ProfileId);

        //        TempData["Message"] = $"Du er nu logget ind som {profile.Email}";

        //        if (string.IsNullOrEmpty(returnUrl))
        //            return RedirectToAction("Index", "Home");
        //        else
        //            return Redirect(returnUrl);
        //    }

        //    ViewBag.ErrorMessage = "Forkert email eller bruger findes ikke.";
        //    return View(loginInfo);
        //}

        //private async Task SignInUsingClaims(List<Claim> claims)
        //{
        //    //create the container for all your claims
        //    //These are stored in the cookie for easy retrieval on the server
        //    var claimsIdentity = new ClaimsIdentity(
        //        claims, CookieAuthenticationDefaults.AuthenticationScheme);

        //    var authProperties = new AuthenticationProperties
        //    {
        //        #region often used options - to consider including in cookie
        //        //AllowRefresh = <bool>,
        //        // Refreshing the authentication session should be allowed.

        //        //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
        //        // The time at which the authentication ticket expires. A 
        //        // value set here overrides the ExpireTimeSpan option of 
        //        // CookieAuthenticationOptions set with AddCookie.

        //        //IsPersistent = true,
        //        // Whether the authentication session is persisted across 
        //        // multiple requests. When used with cookies, controls
        //        // whether the cookie's lifetime is absolute (matching the
        //        // lifetime of the authentication ticket) or session-based.

        //        //IssuedUtc = <DateTimeOffset>,
        //        // The time at which the authentication ticket was issued.

        //        //RedirectUri = <string>
        //        // The full path or absolute URI to be used as an http 
        //        // redirect response value. 
        //        #endregion
        //    };

        //    await HttpContext.SignInAsync(
        //        CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity),
        //        authProperties);
        //}

        [HttpGet]
        public async Task <IActionResult> Login(string? returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }



    }
}
