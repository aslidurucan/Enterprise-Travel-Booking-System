using Rentals.API.Domain.Entities;

namespace Rentals.API.Application.Interfaces
{
    public interface IRentalRepository
    {
        Task<RentableVehicle?> GetVehicleByIdAsync(Guid vehicleId);
        Task<Guid> CreateRentalTransactionAsync(Rental rental, RentableVehicle vehicle);
        Task<IEnumerable<Rental>> GetAllRentalsAsync();
    }
}
