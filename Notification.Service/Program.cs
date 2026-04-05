using MassTransit;
using Notification.Service.Consumers;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<RentalCreatedConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        var rabbitMqHost = builder.Configuration["RabbitMQ:Host"];
        var rabbitMqUser = builder.Configuration["RabbitMQ:Username"];
        var rabbitMqPass = builder.Configuration["RabbitMQ:Password"];

        cfg.Host(rabbitMqHost, "/", h =>
        {
            h.Username(rabbitMqUser);
            h.Password(rabbitMqPass);
        });

        cfg.ReceiveEndpoint("rental-created-queue", e =>
        {
            e.ConfigureConsumer<RentalCreatedConsumer>(context);
        });
    });
});

var host = builder.Build();
host.Run();
