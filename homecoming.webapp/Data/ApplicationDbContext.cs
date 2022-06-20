using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using homecoming.webapp.ViewModel;
using homecoming.api.Model;

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
        public DbSet<homecoming.api.Model.RoomDetail> RoomDetail { get; set; }
    }
}
