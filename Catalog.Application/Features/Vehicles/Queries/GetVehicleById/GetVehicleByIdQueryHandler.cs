using Catalog.Application.DTOs;
using Catalog.Application.Exceptions;
using Catalog.Domain.Repositories;
using MediatR;

namespace Catalog.Application.Features.Vehicles.Queries.GetVehicleById
{
    public class GetVehicleByIdQueryHandler : IRequestHandler<GetVehicleByIdQuery, VehicleDto>
    {
        private readonly IVehicleRepository _repository;

        public GetVehicleByIdQueryHandler(IVehicleRepository repository)
        {
            _repository = repository;
        }

        public async Task<VehicleDto> Handle(GetVehicleByIdQuery request, CancellationToken cancellationToken)
        {
            var vehicle = await _repository.GetByIdAsync(request.Id);


            if (vehicle == null)
            {

                throw new NotFoundException("Aradığınız araç sistemde bulunamadı.");
            }

            var vehicleDto = new VehicleDto
            {
                Id = vehicle.Id,
                Brand = vehicle.Brand,
                Model = vehicle.Model,
                Year = vehicle.Year,
                DailyPrice = vehicle.DailyPrice,
                Currency = vehicle.Currency
                
            };
            return vehicleDto;
        }
    }
}
