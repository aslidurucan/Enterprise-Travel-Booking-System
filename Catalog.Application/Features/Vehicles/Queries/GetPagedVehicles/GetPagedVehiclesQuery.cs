using Catalog.Application.Caching;
using Catalog.Application.DTOs;
using Catalog.Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Catalog.Application.Features.Vehicles.Queries.GetPagedVehicles
{
    public class GetPagedVehiclesQuery : IRequest<PaginatedResult<VehicleDto>> , ICacheableQuery
    { 
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        [BindNever] 
        [JsonIgnore]
        public string CacheKey => $"GetPagedVehicles_{PageIndex}_{PageSize}";

        [BindNever]
        [JsonIgnore]
        public TimeSpan? SlidingExpiration => TimeSpan.FromMinutes(5);
    }
}
