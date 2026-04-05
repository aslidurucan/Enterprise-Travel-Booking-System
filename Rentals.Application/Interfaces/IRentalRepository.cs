using Rentals.Domain.Entities;

namespace Rentals.Application.Interfaces
{
    public interface IRentalRepository
    {
        Task<RentableVehicle?> GetVehicleByIdAsync(Guid vehicleId);
        Task<Guid> CreateRentalTransactionAsync(Rental rental, RentableVehicle vehicle);
        Task<IEnumerable<Rental>> GetAllRentalsAsync();
        Task<bool> HasOverlappingRentalAsync(Guid vehicleId, DateTime startDate, DateTime endDate);
    }
}
