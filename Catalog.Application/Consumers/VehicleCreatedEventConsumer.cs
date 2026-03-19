using MassTransit;
using Microsoft.Extensions.Logging;
using Catalog.Application.Events;

namespace Catalog.Application.Consumers;

public class VehicleCreatedEventConsumer : IConsumer<VehicleCreatedEvent>
{
    private readonly ILogger<VehicleCreatedEventConsumer> _logger;

    public VehicleCreatedEventConsumer(ILogger<VehicleCreatedEventConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<VehicleCreatedEvent> context)
    {
        var message = context.Message;

        _logger.LogInformation($"[RABBITMQ DİNLİYOR 🎧] Yeni araç sisteme eklendi! Marka: {message.Brand}, Model: {message.Model}. Müşteriye bilgi maili gönderiliyor...");

        return Task.CompletedTask;
    }
}