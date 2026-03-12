using Catalog.Application.DTOs;
using Catalog.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Features.Vehicles.Queries.GetAllVehicles
{
    public class GetAllVehiclesQueryHandler : IRequestHandler<GetAllVehiclesQuery, List<VehicleDto>>
    {
        private readonly IVehicleRepository _repository;

        public GetAllVehiclesQueryHandler(IVehicleRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<VehicleDto>> Handle(GetAllVehiclesQuery request, CancellationToken cancellationToken)
        {
            var vehicles = await _repository.GetAllAsync();
            var vehicleDtos = vehicles.Select(v => new VehicleDto
            {
                Id = v.Id,
                Brand = v.Brand,
                Model = v.Model,
                Year = v.Year,
                DailyPrice = v.DailyPrice,
                Currency = v.Currency
            }).ToList();

            return vehicleDtos;
        }
    }
}
