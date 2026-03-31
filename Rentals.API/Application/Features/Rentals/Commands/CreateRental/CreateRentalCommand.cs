using MediatR;

namespace Rentals.API.Application.Features.Rentals.Commands.CreateRental
{
    public class CreateRentalCommand : IRequest<Guid>
    {
        public Guid VehicleId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
