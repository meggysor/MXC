using Microsoft.EntityFrameworkCore;
using MXC.Models;

namespace MXC.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
                : base(options)
        {
        }

        public DbSet<Event> Events { get; set; }
    }
}
