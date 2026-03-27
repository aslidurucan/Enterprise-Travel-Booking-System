using MediatR;
using Rentals.API.Domain.Entities;
using Rentals.API.Infrastructure;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Rentals.API.Application.Features.Rentals.Commands.CreateRental
{
    // MİMARİ KURAL: Bu sınıf CreateRentalCommand isteğini alır ve geriye Guid (Fatura ID'si) döner.
    public class CreateRentalCommandHandler : IRequestHandler<CreateRentalCommand, Guid>
    {
        private readonly RentalsDbContext _context;

        // Dependency Injection ile veritabanı köprümüzü içeri alıyoruz.
        public CreateRentalCommandHandler(RentalsDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Handle(CreateRentalCommand request, CancellationToken cancellationToken)
        {
            var vehicle = await _context.RentableVehicles.FindAsync(new object[] { request.VehicleId }, cancellationToken);

            if (vehicle == null)
            {
                
                throw new Exception("Kiralanamadı: Araç sistemde bulunamadı!");
            }

            if (!vehicle.IsAvailable)
            {
                throw new Exception("Kiralanamadı: Bu araç şu an başka bir müşteride veya bakıma alınmış!");
            }

            if (request.StartDate >= request.EndDate)
            {
                throw new Exception("Kiralanamadı: Bitiş tarihi, başlangıç tarihinden ileri bir zaman olmalıdır!");
            }

            var totalDays = (request.EndDate - request.StartDate).Days;

           
            if (totalDays == 0) totalDays = 1;

            var totalPrice = totalDays * vehicle.DailyPrice;

           
            var rental = new Rental
            {
                Id = Guid.NewGuid(),
                VehicleId = request.VehicleId,
                CustomerId = request.CustomerId, 
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                TotalPrice = totalPrice,
                Status = RentalStatus.Active, 
                CreatedAt = DateTime.UtcNow
            };

            vehicle.IsAvailable = false;

            await _context.Rentals.AddAsync(rental, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return rental.Id;
        }
    }
}