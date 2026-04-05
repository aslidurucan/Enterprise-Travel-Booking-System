namespace EnterpriseTravelBooking.Shared.Events;

public record VehicleUpdatedEvent
{
    public Guid Id { get; init; }
    public decimal DailyPrice { get; init; }
    public string Currency { get; init; } = string.Empty;
}
