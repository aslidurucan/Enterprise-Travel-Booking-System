using Catalog.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Features.Vehicles.Commands.DeleteVehicle
{
    public class DeleteVehicleCommandHandler : IRequestHandler<DeleteVehicleCommand, bool>
    {
        private readonly IVehicleRepository _repository;

        public DeleteVehicleCommandHandler(IVehicleRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(DeleteVehicleCommand request, CancellationToken cancellationToken)
        {
            var vehicleToDelete = await _repository.GetByIdAsync(request.Id);

            if (vehicleToDelete == null)
            {
                throw new Exception("Silinmek istenen araç bulunamadı!");
            }
            await _repository.DeleteAsync(vehicleToDelete);

            return true; 
        }
    }
}
