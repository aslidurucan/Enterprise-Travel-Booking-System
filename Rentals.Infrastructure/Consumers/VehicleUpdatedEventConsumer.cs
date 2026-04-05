using EnterpriseTravelBooking.Shared.Events;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Rentals.Infrastructure.Persistence;

namespace Rentals.Infrastructure.Consumers
{
    public class VehicleUpdatedEventConsumer : IConsumer<VehicleUpdatedEvent>
    {
        private readonly RentalsDbContext _context;
        private readonly ILogger<VehicleUpdatedEventConsumer> _logger;

        public VehicleUpdatedEventConsumer(RentalsDbContext context, ILogger<VehicleUpdatedEventConsumer> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<VehicleUpdatedEvent> context)
        {
            var message = context.Message;

            var vehicle = await _context.RentableVehicles
                .FirstOrDefaultAsync(v => v.Id == message.Id);

            if (vehicle is null)
            {
                _logger.LogWarning("[RENTALS] Güncellenecek araç bulunamadı. VehicleId: {Id}", message.Id);
                return;
            }

            vehicle.DailyPrice = message.DailyPrice;
            vehicle.Currency = message.Currency;

            await _context.SaveChangesAsync();

            _logger.LogInformation("[RENTALS] Araç fiyatı güncellendi. VehicleId: {Id}", message.Id);
        }
    }
}
