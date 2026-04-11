using Microsoft.EntityFrameworkCore;

namespace Radisson_RHG.Controllers
{
    public class ApplicationDbContext: DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {

        }

        public DbSet<Registration> registrations { get; set; }
    }
}
