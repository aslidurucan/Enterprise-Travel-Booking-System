using MediatR;

namespace Rentals.API.Application.Features.Rentals.Commands.CreateRental
{
    public class CreateRentalCommand : IRequest<Guid>
    {
        public Guid VehicleId { get; set; }
        public string CustomerId { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
