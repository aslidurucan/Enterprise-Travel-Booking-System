using MassTransit;
using Rentals.API.Domain.Entities;
using Rentals.API.Infrastructure;
using Catalog.Application.Events;
using Microsoft.EntityFrameworkCore;

namespace Rentals.API.Application.Consumers
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
            _logger.LogInformation($"[RENTALS.API] Yeni araç anonsu duyuldu! Katalog ID: {message.Id}");


            var vehicleExists = await _context.RentableVehicles
                 .AnyAsync(v => v.Id == message.Id);

            if (vehicleExists)
            {
                _logger.LogWarning($"[RENTALS.API] {message.Id} ID'li araç zaten sistemde kayıtlı. İşlem atlanıyor.");
                return;
            }

            if (message.DailyPrice <= 0)
            {
                _logger.LogError($"[RENTALS.API] HATA: {message.Id} ID'li aracın günlük fiyatı geçersiz ({message.DailyPrice}). Araç garaja EKLENMEDİ!");
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

            _logger.LogInformation($"[RENTALS.API] ZAFER! Araç başarıyla RentalsDb'ye kopyalandı! Kiralama için hazır. 🚗");
        }
    }
}


