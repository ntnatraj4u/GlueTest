using Microsoft.EntityFrameworkCore;

namespace DeliveryLogic.Models
{
    public class DeliveryContext : DbContext
    {
        public DeliveryContext(DbContextOptions<DeliveryContext> options) : base(options)
        {
        }

        public DbSet<Delivery> Deliveries { get; set; }

        public DbSet<AccessWindow> AccessWindows { get; set; }

        public DbSet<Recipient> Recipients { get; set; }

        public DbSet<Order> Orders { get; set; }

    }
}
