using Microsoft.AspNetCore.Mvc;
using ReelWorld.DataAccessLibrary.Interfaces;
using ReelWorld.DataAccessLibrary.Model;
using ReelWorld.DataAccessLibrary.SqlServer;

namespace ReelWorld.Api.Controllers
{
    
        [ApiController]
        [Route("api/[controller]")]
        public class LoginsController : ControllerBase
        {
            private readonly ILoginDao _loginRepository;

            public LoginsController(ILoginDao loginRepo)
            {
                _loginRepository = loginRepo;
            }

            [HttpPost("login")]
            public async Task<ActionResult<int>> Login([FromBody] Profile profile)
            {
                int userId = await _loginRepository.LoginAsync(profile.Email, profile.HashPassword);

                if (userId <= 0)
                    return Unauthorized();

                return Ok(userId);
            }
        }
}

