using MediatR;
using Rentals.Application.DTOs;

namespace Rentals.Application.Features.Rentals.Queries.GetAllRentals
{
    public class GetAllRentalsQuery : IRequest<List<RentalDto>>
    {
    }
}
