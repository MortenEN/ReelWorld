using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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

        [HttpGet("{id}")]
        public async Task<ActionResult<List<Profile>>> GetOneAsync(int id)
        {
            try
            {
                var profile = await _profileDao.GetOneAsync(id);
                return Ok(profile);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
                throw;
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<bool>> Update(Profile profile)
        {
            try
            {
                var result = await _profileDao.UpdateAsync(profile);
                return Ok(profile);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<Profile>>> GetAll()
        {
            try
            {
                var profiles = await _profileDao.GetAllAsync();
                return Ok(profiles);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}
