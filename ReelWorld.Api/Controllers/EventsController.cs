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
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvent(int id, Event updatedEvent)
        {
            if (id != updatedEvent.EventId)
                return BadRequest("Event ID mismatch");

            var result = await _eventDao.UpdateAsync(updatedEvent);

            if (!result)
                return NotFound();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            try
            {
                var delete = await _eventDao.DeleteAsync(id);
                return Ok(delete);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet("Search")]
        public async Task<ActionResult<List<Event>>> Search([FromQuery] string query, [FromQuery] string? category)
        {
            try
            {
                var result = await _eventDao.SearchAsync(query, category);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }


        [HttpGet("Biggest")]
        public async Task<ActionResult<List<Event>>> Get10BiggestAsync()
        {
            try
            {
                var events = await _eventDao.Get10BiggestAsync();
                return Ok(events);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

    }
}