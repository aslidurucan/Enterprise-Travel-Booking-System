using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseTravelBooking.Shared.Events;

public record VehicleCreatedEvent
{
    public Guid Id { get; init; }
    public string Brand { get; init; } = string.Empty;
    public string Model { get; init; } = string.Empty;
    public int Year { get; init; }
    public decimal DailyPrice { get; init; }
    public string Currency { get; init; } = "TL";
}
