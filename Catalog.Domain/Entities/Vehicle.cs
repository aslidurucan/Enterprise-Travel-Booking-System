using Catalog.Domain.Common;

namespace Catalog.Domain.Entities
{
    public class Vehicle : BaseEntity, ISoftDeletable
    {
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int Year { get; set; }
        public decimal DailyPrice { get; set; }
        public string Currency { get; set; } = string.Empty;
        public bool IsAvailable { get; set; }
        public bool IsDeleted { get; set; } = false;

        public Vehicle()
        {
            IsAvailable = true;
        }
    }
}
