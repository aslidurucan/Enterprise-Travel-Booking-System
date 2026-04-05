namespace EnterpriseTravelBooking.Shared.Events;

public record VehicleDeletedEvent
{
    public Guid Id { get; init; }
}
