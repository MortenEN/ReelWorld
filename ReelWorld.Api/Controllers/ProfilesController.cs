using Microsoft.AspNetCore.Mvc;
using ReelWorld.DataAccessLibrary.Interfaces;
using ReelWorld.DataAccessLibrary.Model;

namespace ReelWorld.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfilesController : ControllerBase
    {
        private readonly IProfileDaoAsync _profileDao;

        public ProfilesController(IProfileDaoAsync profileDao)
        {
            _profileDao = profileDao;
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create(Profile profile)
        {
            try
            {
                var id = await _profileDao.CreateAsync(profile);
                return Ok(id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            try
            {
                var delete = await _profileDao.DeleteAsync(id);
                return Ok(delete);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}
