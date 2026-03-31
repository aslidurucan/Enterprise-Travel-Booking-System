using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Features.Vehicles.Commands.UpdateVehicle
{
    public class UpdateVehicleCommand : IRequest<bool>
    {
        public Guid Id { get; set; } 
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public decimal DailyPrice { get; set; }
        public string Currency { get; set; }
    }
}
