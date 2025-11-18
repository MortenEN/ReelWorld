using Microsoft.AspNetCore.Mvc;
using ReelWorld.DataAccessLibrary.Interfaces;
using ReelWorld.DataAccessLibrary.Model;

namespace ReelWorld.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        IUserDaoAsync _userDao;
        public UsersController(IUserDaoAsync userDao)
        {
            _userDao = userDao;
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create(User user)
        {
            try
            {
                var id = await _userDao.CreateAsync(user);
                return Ok(id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}