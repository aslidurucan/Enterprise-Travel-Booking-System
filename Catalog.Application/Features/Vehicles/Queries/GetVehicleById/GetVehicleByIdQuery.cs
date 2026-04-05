using Catalog.Application.Caching;
using Catalog.Application.DTOs;
using MediatR;

namespace Catalog.Application.Features.Vehicles.Queries.GetVehicleById
{
    public class GetVehicleByIdQuery : IRequest<VehicleDto>, ICacheableQuery
    {
        public Guid Id { get; set; }

        public string CacheKey => $"Vehicle_{Id}";
        public bool BypassCache => false;
        public TimeSpan? SlidingExpiration => TimeSpan.FromMinutes(10);
    }
}
