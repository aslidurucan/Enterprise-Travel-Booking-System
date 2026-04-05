using Microsoft.EntityFrameworkCore;
using Rentals.Application.Interfaces;
using Rentals.Domain.Entities;
using Rentals.Infrastructure.Persistence;

namespace Rentals.Infrastructure.Repositories
{
    public class RentalRepository : IRentalRepository
    {
        private readonly RentalsDbContext _context;

        public RentalRepository(RentalsDbContext context) => _context = context;

        public async Task<RentableVehicle?> GetVehicleByIdAsync(Guid vehicleId)
            => await _context.RentableVehicles.FindAsync(vehicleId);

        public async Task<Guid> CreateRentalTransactionAsync(Rental rental, RentableVehicle vehicle)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                vehicle.IsAvailable = false;
                await _context.Rentals.AddAsync(rental);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return rental.Id;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<IEnumerable<Rental>> GetAllRentalsAsync()
            => await _context.Rentals.AsNoTracking().ToListAsync();

        public async Task<bool> HasOverlappingRentalAsync(Guid vehicleId, DateTime startDate, DateTime endDate)
        {
            return await _context.Rentals.AnyAsync(r =>
                r.VehicleId == vehicleId &&
                r.Status != RentalStatus.Cancelled &&
                r.Status != RentalStatus.Completed &&
                startDate < r.EndDate &&
                endDate > r.StartDate);
        }
    }
}
