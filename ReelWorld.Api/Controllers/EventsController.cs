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
        IEventDaoAsync _eventDao;
        public EventsController(IEventDaoAsync eventDao)
        {
            _eventDao = eventDao;
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create(Event @event)
        {
            try
            {
                var id = await _eventDao.CreateAsync(@event);
                return Ok(id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
        [HttpGet]
        public async Task<ActionResult<List<Event>>> GetAll()
        {
            try
            {
                var events = await _eventDao.GetAllAsync();
                return Ok(events);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
        [HttpGet("Latest")]
        public async Task<ActionResult<List<Event>>> Get10LatestAsync()
        {
            try
            {
                var events = await _eventDao.Get10LatestAsync();
                return Ok(events);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<List<Event>>> GetOneAsync(int id)
        {
            try
            {
                var events = await _eventDao.GetOneAsync(id);
                return Ok(events);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
                throw;
            }
        }

        [HttpPost("{eventId}/join/{userId}")]
        public async Task<bool> JoinEventAsync(int eventId, int UserId)
        {
            try
            {
                var result = await _eventDao.JoinEventAsync(eventId, UserId);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Internal server error: " + ex.Message);
            }

        }

    }
}