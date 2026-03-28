using MediatR;
using Microsoft.EntityFrameworkCore;
using Rentals.API.Domain.Entities;
using Rentals.API.Infrastructure;

namespace Rentals.API.Application.Features.Rentals.Queries.GetAllRentals
{
    public class GetAllRentalsQueryHandler : IRequestHandler<GetAllRentalsQuery, List<Rental>>
    {
        private readonly RentalsDbContext _context;

        public GetAllRentalsQueryHandler(RentalsDbContext context)
        {
            _context = context;
        }

        public async Task<List<Rental>> Handle(GetAllRentalsQuery request, CancellationToken cancellationToken)
        {
            var rentals = await _context.Rentals
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return rentals;
        }

    }
}
