using Catalog.Application.Exceptions;
using Catalog.Domain.Repositories;
using EnterpriseTravelBooking.Shared.Events;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;

namespace Catalog.Application.Features.Vehicles.Commands.DeleteVehicle
{
    public class DeleteVehicleCommandHandler : IRequestHandler<DeleteVehicleCommand, bool>
    {
        private readonly IVehicleRepository _repository;
        private readonly IDistributedCache _cache;
        private readonly IPublishEndpoint _publishEndpoint;

        public DeleteVehicleCommandHandler(IVehicleRepository repository, IDistributedCache cache, IPublishEndpoint publishEndpoint)
        {
            _repository = repository;
            _cache = cache;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<bool> Handle(DeleteVehicleCommand request, CancellationToken cancellationToken)
        {
            var vehicleToDelete = await _repository.GetByIdAsync(request.Id);

            if (vehicleToDelete == null)
                throw new NotFoundException("Silinmek istenen araç bulunamadı.");

            await _repository.DeleteAsync(vehicleToDelete);
            await _cache.RemoveAsync("VehiclesList", cancellationToken);
            await _cache.RemoveAsync($"Vehicle_{request.Id}", cancellationToken);

            await _publishEndpoint.Publish(new VehicleDeletedEvent
            {
                Id = request.Id
            }, cancellationToken);

            return true;
        }
    }
}
