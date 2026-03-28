using MediatR;
using Rentals.API.Domain.Entities;

namespace Rentals.API.Application.Features.Rentals.Queries.GetAllRentals
{
    public class GetAllRentalsQuery : IRequest<List<Rental>>
    {

    }
}
