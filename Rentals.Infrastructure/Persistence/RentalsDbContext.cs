using Microsoft.EntityFrameworkCore;
using Rentals.Domain.Entities;

namespace Rentals.Infrastructure.Persistence
{
    public class RentalsDbContext : DbContext
    {
        public RentalsDbContext(DbContextOptions<RentalsDbContext> options) : base(options)
        {
        }

        public DbSet<Rental> Rentals { get; set; }
        public DbSet<RentableVehicle> RentableVehicles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<RentableVehicle>()
                .Property(v => v.Id)
                .ValueGeneratedNever();
        }
    }
}
