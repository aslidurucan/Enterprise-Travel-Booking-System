using Catalog.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Features.Vehicles.Queries.GetAllVehicles
{
    public class GetAllVehiclesQuery : IRequest<List<VehicleDto>>
    {
    }
}
