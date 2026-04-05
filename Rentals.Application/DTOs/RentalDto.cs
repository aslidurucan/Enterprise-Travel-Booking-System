namespace Rentals.Application.DTOs
{
    public class RentalDto
    {
        public Guid Id { get; set; }
        public Guid VehicleId { get; set; }
        public string CustomerId { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
