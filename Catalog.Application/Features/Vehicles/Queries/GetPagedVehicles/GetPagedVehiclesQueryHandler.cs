using Catalog.Application.DTOs;
using Catalog.Domain.Common;
using Catalog.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Features.Vehicles.Queries.GetPagedVehicles
{
    public class GetPagedVehiclesQueryHandler : IRequestHandler<GetPagedVehiclesQuery, PaginatedResult<VehicleDto>>
    {
        private readonly IVehicleRepository _repository;

        public GetPagedVehiclesQueryHandler(IVehicleRepository repository)
        {
            _repository = repository;
        }

        public async Task<PaginatedResult<VehicleDto>> Handle(GetPagedVehiclesQuery request, CancellationToken cancellationToken)
        {
            if (request.PageSize > 50)
            {
                request.PageSize = 50;
            }

            if (request.PageIndex < 1)
            {
                request.PageIndex = 1; 
            }

            var rawPagedResult = await _repository.GetPagedAsync(request.PageIndex, request.PageSize);

            var dtoList = rawPagedResult.Items.Select(vehicle => new VehicleDto
            {
                Id = vehicle.Id,
                Brand = vehicle.Brand,
                Model = vehicle.Model,
                Year = vehicle.Year,
                DailyPrice = vehicle.DailyPrice,
                Currency = vehicle.Currency
            }).ToList();

            var safePagedResult = new PaginatedResult<VehicleDto>(
                dtoList,
                rawPagedResult.TotalCount,
                rawPagedResult.PageIndex,
                rawPagedResult.PageSize);

            return safePagedResult;
        }
    }
}
