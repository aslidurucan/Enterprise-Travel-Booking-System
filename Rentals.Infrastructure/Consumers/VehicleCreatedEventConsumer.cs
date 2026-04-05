using EnterpriseTravelBooking.Shared.Events;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Rentals.Domain.Entities;
using Rentals.Infrastructure.Persistence;

namespace Rentals.Infrastructure.Consumers
{
    public class VehicleCreatedEventConsumer : IConsumer<VehicleCreatedEvent>
    {
        private readonly RentalsDbContext _context;
        private readonly ILogger<VehicleCreatedEventConsumer> _logger;

        public VehicleCreatedEventConsumer(RentalsDbContext context, ILogger<VehicleCreatedEventConsumer> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<VehicleCreatedEvent> context)
        {
            var message = context.Message;
            _logger.LogInformation("[RENTALS] Yeni araç anonsu duyuldu! Katalog ID: {Id}", message.Id);

            var vehicleExists = await _context.RentableVehicles
                .AnyAsync(v => v.Id == message.Id);

            if (vehicleExists)
            {
                _logger.LogWarning("[RENTALS] {Id} ID'li araç zaten sistemde kayıtlı. İşlem atlanıyor.", message.Id);
                return;
            }

            if (message.DailyPrice <= 0)
            {
                _logger.LogError("[RENTALS] HATA: {Id} ID'li aracın günlük fiyatı geçersiz ({Price}). Araç EKLENMEDİ!", message.Id, message.DailyPrice);
                return;
            }

            var rentableVehicle = new RentableVehicle
            {
                Id = message.Id,
                DailyPrice = message.DailyPrice,
                Currency = message.Currency,
                IsAvailable = true
            };

            await _context.RentableVehicles.AddAsync(rentableVehicle);
            await _context.SaveChangesAsync();

            _logger.LogInformation("[RENTALS] Araç başarıyla RentalsDb'ye kopyalandı. VehicleId: {Id}", message.Id);
        }
    }
}
