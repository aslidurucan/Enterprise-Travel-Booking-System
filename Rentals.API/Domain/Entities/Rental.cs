namespace Rentals.API.Domain.Entities
{
    public class Rental
    {
        public Guid Id { get; set; }
        public Guid VehicleId { get; set; } 
        public string CustomerName { get; set; } 
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; } 
    }
}
