namespace Catalog.Application.Events
{
    public class VehicleCreatedEvent
    {
        public Guid Id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public decimal DailyPrice { get; set; }
        public string Currency { get; set; }
    }
}
