using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseTravelBooking.Shared.Events;

public record RentalCreatedEvent
{
    public Guid RentalId { get; init; }
    public string CustomerEmail { get; init; }
    public string VehicleInfo { get; init; }
    public decimal TotalPrice { get; init; }
    public DateTime CreatedAt { get; init; }
}