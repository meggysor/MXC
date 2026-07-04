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
    }
}
