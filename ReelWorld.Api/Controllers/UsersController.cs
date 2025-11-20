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

        [HttpDelete("{id}")]
        public async Task <ActionResult<bool>> Delete(int id)
        {
            try
            {
                var delete = await _userDao.DeleteAsync(id);
                return Ok(delete);
            }

            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

    }
}