using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Features.Vehicles.Commands.DeleteVehicle
{
    public class DeleteVehicleCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}
