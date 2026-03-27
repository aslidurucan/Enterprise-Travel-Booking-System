namespace Rentals.API.Domain.Entities
{

    public enum RentalStatus
    {
        Pending,
        Active,
        Completed,
        Cancelled
    }
    public class Rental
    {
        public Guid Id { get; set; }
        public Guid VehicleId { get; set; } 
        public string CustomerId { get; set; } 
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalPrice { get; set; }
        public RentalStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
