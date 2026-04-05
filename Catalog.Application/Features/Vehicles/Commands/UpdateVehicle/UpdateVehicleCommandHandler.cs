using Catalog.Application.Exceptions;
using Catalog.Domain.Repositories;
using EnterpriseTravelBooking.Shared.Events;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;

namespace Catalog.Application.Features.Vehicles.Commands.UpdateVehicle
{
    public class UpdateVehicleCommandHandler : IRequestHandler<UpdateVehicleCommand, bool>
    {
        private readonly IVehicleRepository _repository;
        private readonly IDistributedCache _cache;
        private readonly IPublishEndpoint _publishEndpoint;

        public UpdateVehicleCommandHandler(IVehicleRepository repository, IDistributedCache cache, IPublishEndpoint publishEndpoint)
        {
            _repository = repository;
            _cache = cache;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<bool> Handle(UpdateVehicleCommand request, CancellationToken cancellationToken)
        {
            var vehicleToUpdate = await _repository.GetByIdAsync(request.Id);

            if (vehicleToUpdate == null)
                throw new NotFoundException("Güncellenmek istenen araç bulunamadı.");

            vehicleToUpdate.Brand = request.Brand;
            vehicleToUpdate.Model = request.Model;
            vehicleToUpdate.Year = request.Year;
            vehicleToUpdate.DailyPrice = request.DailyPrice;
            vehicleToUpdate.Currency = request.Currency;

            await _repository.UpdateAsync(vehicleToUpdate);
            await _cache.RemoveAsync("VehiclesList", cancellationToken);
            await _cache.RemoveAsync($"Vehicle_{request.Id}", cancellationToken);

            await _publishEndpoint.Publish(new VehicleUpdatedEvent
            {
                Id = vehicleToUpdate.Id,
                DailyPrice = vehicleToUpdate.DailyPrice,
                Currency = vehicleToUpdate.Currency
            }, cancellationToken);

            return true;
        }
    }
}
