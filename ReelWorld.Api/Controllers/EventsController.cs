using Microsoft.AspNetCore.Mvc;
using ReelWorld.DataAccessLibrary.Interfaces;
using ReelWorld.DataAccessLibrary.Model;
using ReelWorld.DataAccessLibrary.SqlServer;

namespace ReelWorld.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        IEventDao _eventDao;
        public EventsController(IEventDao eventDao)
        {
            _eventDao = eventDao;
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create(Event event)
        {
            try
            {
                var id = await _eventDao.CreateEventAsync(event);
                return Ok(id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}