using Microsoft.AspNetCore.Mvc;
using ReelWorld.DataAccessLibrary.Interfaces;

namespace ReelWorld.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegistrationsController : ControllerBase
    {
        IRegistrationDaoAsync _registrationDao;
        public RegistrationsController(IRegistrationDaoAsync registrationDao)
        {
            _registrationDao = registrationDao;
        }

        [HttpPost("{eventId}/join/{ProfileId}")]
        public async Task<bool> JoinEventAsync(int eventId, int ProfileId)
        {
            try
            {
                var result = await _registrationDao.JoinEventAsync(eventId, ProfileId);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Internal server error: " + ex.Message);
            }
        }
    }
}
