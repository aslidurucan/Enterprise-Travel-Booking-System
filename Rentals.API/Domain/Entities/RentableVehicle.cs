namespace Rentals.API.Domain.Entities
{
    public class RentableVehicle
    {
        public Guid Id { get; set; }
        public decimal DailyPrice { get; set; }
        public bool IsAvailable { get; set; }
        public string Currency { get; set; }
    }
}
