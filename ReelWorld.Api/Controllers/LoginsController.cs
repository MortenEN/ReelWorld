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
        #region Repositories and Constructors
        private readonly IProfileDaoAsync _profileRepository;
        public LoginsController(IProfileDaoAsync repository) => _profileRepository = repository;
        #endregion

        [HttpPost]
        public async Task<ActionResult<int>> Post([FromBody] Profile loginvalues) => await _profileRepository.LoginAsync(loginvalues.Email, loginvalues.HashPassword);
    }
}
