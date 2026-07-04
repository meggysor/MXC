using Microsoft.AspNetCore.Mvc;
using MXC.Data;
using MXC.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace MXC.Controllers
{
    [ApiController]
    [Route("api")]
    [Authorize]
    public class EventsController : ControllerBase
    {
        private readonly ILogger<EventsController> _logger;
        private readonly ApplicationDbContext _dbContext;

        public EventsController(ILogger<EventsController> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var allEvents = await _dbContext.Events.ToListAsync();

            return Ok(allEvents);
        }

        [HttpGet]
        [Route("GetById")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var record = await _dbContext.Events.FindAsync(id);

            if (record == null)
            {
                _logger.LogWarning("Event with ID {EventId} not found.", id);
                return NotFound();
            }

            return Ok(record);
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create([FromBody] Event newEvent)
        {
            if (newEvent == null)
            {
                _logger.LogWarning("Create request received with null event.");
                return BadRequest("Event data is required.");
            }
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Create request received with invalid model state.");
                return BadRequest(ModelState);
            }

            _dbContext.Events.Add(newEvent);
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation("Creating new event with ID {EventId}.", newEvent.Id);

            return CreatedAtAction(nameof(GetById), new { id = newEvent.Id }, newEvent);
        }

        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> Update([FromBody] Event updatedEvent)
        {
            if (updatedEvent == null)
            {
                _logger.LogWarning("Update request received with null event.");
                return BadRequest("Event data is required.");
            }
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Update request received with invalid model state.");
                return BadRequest(ModelState);
            }

            var existingEvent = await _dbContext.Events.FindAsync(updatedEvent.Id);

            if (existingEvent == null)
                return NotFound();

            existingEvent.Name = updatedEvent.Name;
            existingEvent.Location = updatedEvent.Location;
            existingEvent.Country = updatedEvent.Country;
            existingEvent.Capacity = updatedEvent.Capacity;

            await _dbContext.SaveChangesAsync();
            _logger.LogInformation("Updated event with ID {EventId}.", updatedEvent.Id);

            return NoContent();
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var existingEvent = await _dbContext.Events.FindAsync(id);

            if (existingEvent == null)
                return NotFound();

            _dbContext.Events.Remove(existingEvent);
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation("Deleted event with ID {EventId}.", id);

            return NoContent();
        }
    }
}
