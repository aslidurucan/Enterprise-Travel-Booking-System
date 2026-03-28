using System;
using System.Text.Json.Serialization;

namespace Catalog.Application.Events
{
    public record VehicleCreatedEvent
    {
        public Guid Id { get; init; }
        public string Brand { get; init; }
        public string Model { get; init; }
        public int Year { get; init; }

        public decimal DailyPrice { get; init; }

        [JsonPropertyName("currency")]
        public string Currency { get; init; }
    }
}