using System.ComponentModel.DataAnnotations;

namespace MXC.Models
{
    public sealed class Event
    {
        public Guid Id { get; set; }

        public required string Name { get; set; } = string.Empty;

        [MaxLength(100)]
        public required string Location { get; set; } = string.Empty;

        public string? Country { get; set; }

        [Range(1, 100)]
        public int? Capacity { get; set; }
    }
}
