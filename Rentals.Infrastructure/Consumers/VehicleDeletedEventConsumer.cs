using EnterpriseTravelBooking.Shared.Events;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Rentals.Infrastructure.Persistence;

namespace Rentals.Infrastructure.Consumers
{
    public class VehicleDeletedEventConsumer : IConsumer<VehicleDeletedEvent>
    {
        private readonly RentalsDbContext _context;
        private readonly ILogger<VehicleDeletedEventConsumer> _logger;

        public VehicleDeletedEventConsumer(RentalsDbContext context, ILogger<VehicleDeletedEventConsumer> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<VehicleDeletedEvent> context)
        {
            var message = context.Message;

            var vehicle = await _context.RentableVehicles
                .FirstOrDefaultAsync(v => v.Id == message.Id);

            if (vehicle is null)
            {
                _logger.LogWarning("[RENTALS] Silinecek araç bulunamadı. VehicleId: {Id}", message.Id);
                return;
            }

            _context.RentableVehicles.Remove(vehicle);
            await _context.SaveChangesAsync();

            _logger.LogInformation("[RENTALS] Araç RentalsDb'den silindi. VehicleId: {Id}", message.Id);
        }
    }
}
