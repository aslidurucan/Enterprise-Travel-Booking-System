namespace EnterpriseTravelBooking.Shared.Events;

public record RentalCreatedEvent
{
    public Guid RentalId { get; init; }
    public string CustomerEmail { get; init; } = string.Empty;
    public string VehicleInfo { get; init; } = string.Empty;
    public decimal TotalPrice { get; init; }
    public DateTime CreatedAt { get; init; }
}
