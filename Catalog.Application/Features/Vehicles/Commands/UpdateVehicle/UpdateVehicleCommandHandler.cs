using Catalog.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Features.Vehicles.Commands.UpdateVehicle
{
    public class UpdateVehicleCommandHandler : IRequestHandler<UpdateVehicleCommand, bool>
    {
        private readonly IVehicleRepository _repository;

        public UpdateVehicleCommandHandler(IVehicleRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(UpdateVehicleCommand request, CancellationToken cancellationToken)
        {
            var vehicleToUpdate = await _repository.GetByIdAsync(request.Id);

            if (vehicleToUpdate == null)
            {
                throw new Exception("Güncellenmek istenen araç bulunamadı!");
            }

            vehicleToUpdate.Brand = request.Brand;
            vehicleToUpdate.Model = request.Model;
            vehicleToUpdate.Year = request.Year;
            vehicleToUpdate.DailyPrice = request.DailyPrice;
            vehicleToUpdate.Currency = request.Currency;

            await _repository.UpdateAsync(vehicleToUpdate);

            return true; 
        }
    }
}
