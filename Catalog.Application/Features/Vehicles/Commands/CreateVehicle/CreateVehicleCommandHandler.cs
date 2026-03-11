using Catalog.Domain.Entities;
using Catalog.Domain.Repositories;
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

        public CreateVehicleCommandHandler(IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
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

            return vehicle.Id;
        }
    }
}
