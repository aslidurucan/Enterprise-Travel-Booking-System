using Microsoft.EntityFrameworkCore;
using Rentals.API.Application.Interfaces;
using Rentals.API.Domain.Entities;
using Rentals.API.Infrastructure;

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
            vehicle.IsAvailable = false; // Aracı kilitle
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
        => await _context.Rentals.ToListAsync();
}