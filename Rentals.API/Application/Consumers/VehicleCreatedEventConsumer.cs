using MassTransit;
using Rentals.API.Domain.Entities;
using Rentals.API.Infrastructure;
using Catalog.Application.Events;

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

            _logger.LogInformation($"[RENTALS.API] Yeni araç yakalandı! Katalogdan gelen ID: {message.Id}");

            
            var rentableVehicle = new RentableVehicle
            {
                Id = message.Id, 
                DailyPrice = message.DailyPrice,
                Currency = message.Currency,
                IsAvailable = true 
            };

            await _context.RentableVehicles.AddAsync(rentableVehicle);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"[RENTALS.API] Araç başarıyla RentalsDb'ye kopyalandı! Kiralama için hazır. 🚗");
        }
    }

}
