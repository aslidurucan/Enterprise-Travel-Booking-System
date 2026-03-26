using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Events
{
    public record VehicleCreatedEvent
    {
        public Guid Id { get; init; }
        public string Brand { get; init; }
        public string Model { get; init; }
        public int Year { get; internal set; }
        public decimal DailyPrice { get; internal set; }
        public string Currency { get; set; }
    }
}
