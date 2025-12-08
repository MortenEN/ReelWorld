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

        [HttpPost]
        public async Task<ActionResult<int>> Login([FromBody] LoginDto login)
        {
            if (string.IsNullOrEmpty(login.Email) || string.IsNullOrEmpty(login.Password))
                return BadRequest("Email og Password skal udfyldes.");

            int userId = await _loginRepository.LoginAsync(login.Email, login.Password);

            if (userId <= 0)
                return Unauthorized();

            return Ok(userId);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<List<Profile>>> GetOneAsync(int id)
        {
            try
            {
                var profile = await _loginRepository.GetOneAsync(id);
                return Ok(profile);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
                throw;
            }
        }
    }
}

