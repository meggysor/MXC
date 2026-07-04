using Microsoft.AspNetCore.Mvc;
using MXC.Models;

namespace MXC.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventsController : ControllerBase
    {
        private static readonly List<Event> _testEvents =
        [
            new Event
            {
                Id = Guid.NewGuid(),
                Name = "Sample Event 1",
                Location = "New York",
                Country = "USA",
                Capacity = 50
            },
            new Event
            {
                Id = Guid.NewGuid(),
                Name = "Sample Event 2",
                Location = "London",
                Country = "UK",
                Capacity = 75
            }
        ];

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            await Task.Delay(1000); // Simulate some async work

            return Ok(_testEvents);
        }

        [HttpGet]
        [Route("GetById")]
        public async Task<IActionResult> GetById(Guid id)
        {
            await Task.Delay(1000); // Simulate some async work

            var record = _testEvents.FirstOrDefault(e => e.Id == id);

            if (record == null)
                return NotFound();

            return Ok(record);
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create([FromBody] Event newEvent)
        {
            await Task.Delay(1000); // Simulate some async work

            newEvent.Id = Guid.NewGuid();
            _testEvents.Add(newEvent);
            return CreatedAtAction(nameof(GetById), new { id = newEvent.Id }, newEvent);
        }

        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> Update([FromBody] Event updatedEvent)
        {
            await Task.Delay(1000); // Simulate some async work
            var existingEvent = _testEvents.FirstOrDefault(e => e.Id == updatedEvent.Id);

            if (existingEvent == null)
                return NotFound();

            existingEvent.Name = updatedEvent.Name;
            existingEvent.Location = updatedEvent.Location;
            existingEvent.Country = updatedEvent.Country;
            existingEvent.Capacity = updatedEvent.Capacity;

            return NoContent();
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await Task.Delay(1000); // Simulate some async work

            var existingEvent = _testEvents.FirstOrDefault(e => e.Id == id);
            if (existingEvent == null)
                return NotFound();
            _testEvents.Remove(existingEvent);

            return Ok();
        }
    }
}
