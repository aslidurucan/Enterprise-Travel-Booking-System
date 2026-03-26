using Microsoft.EntityFrameworkCore;
using Rentals.API.Domain.Entities;

namespace Rentals.API.Infrastructure
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
