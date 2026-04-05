using MediatR;
using Rentals.Application.DTOs;
using Rentals.Application.Interfaces;

namespace Rentals.Application.Features.Rentals.Queries.GetAllRentals
{
    public class GetAllRentalsQueryHandler : IRequestHandler<GetAllRentalsQuery, List<RentalDto>>
    {
        private readonly IRentalRepository _rentalRepository;

        public GetAllRentalsQueryHandler(IRentalRepository rentalRepository)
        {
            _rentalRepository = rentalRepository;
        }

        public async Task<List<RentalDto>> Handle(GetAllRentalsQuery request, CancellationToken cancellationToken)
        {
            var rentals = await _rentalRepository.GetAllRentalsAsync();

            return rentals.Select(r => new RentalDto
            {
                Id = r.Id,
                VehicleId = r.VehicleId,
                CustomerId = r.CustomerId,
                StartDate = r.StartDate,
                EndDate = r.EndDate,
                TotalPrice = r.TotalPrice,
                Status = r.Status.ToString()
            }).ToList();
        }
    }
}
