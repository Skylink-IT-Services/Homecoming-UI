using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using homecoming.webapp.ViewModel;

namespace homecoming.webapp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<RoomTypeViewModel> RoomDetails { get; set; }
        public DbSet<RoomViewModel> Rooms { get; set; }
    }
}
