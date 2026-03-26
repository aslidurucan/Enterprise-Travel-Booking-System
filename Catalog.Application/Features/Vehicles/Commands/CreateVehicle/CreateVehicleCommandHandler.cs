using Catalog.Application.Events;
using Catalog.Domain.Entities;
using Catalog.Domain.Repositories;
using MassTransit;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Catalog.Application.Features.Vehicles.Commands.CreateVehicle
{
    public class CreateVehicleCommandHandler : IRequestHandler<CreateVehicleCommand, Guid>
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IPublishEndpoint _publishEndpoint;

        public CreateVehicleCommandHandler(IVehicleRepository vehicleRepository, IPublishEndpoint publishEndpoint)
        {
            _vehicleRepository = vehicleRepository;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<Guid> Handle(CreateVehicleCommand request, CancellationToken cancellationToken)
        {
           
            var vehicle = new Vehicle
            {
                Id = Guid.NewGuid(), 
                CreatedDate = DateTime.UtcNow,
                Brand = request.Brand,
                Model = request.Model,
                Year = request.Year,
                DailyPrice = request.DailyPrice,
                Currency = request.Currency,
                IsAvailable = true
            };

            await _vehicleRepository.AddAsync(vehicle);

            await _publishEndpoint.Publish(new VehicleCreatedEvent
            {
                Id = vehicle.Id,
                Brand = vehicle.Brand,
                Model = vehicle.Model,
                Year = vehicle.Year, 
                DailyPrice = vehicle.DailyPrice,
                Currency = vehicle.Currency
            }, cancellationToken);

            return vehicle.Id;
        }
    }
}
