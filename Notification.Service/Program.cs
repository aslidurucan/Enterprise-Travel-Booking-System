using MassTransit;
using Notification.Service.Consumers;

var builder = Host.CreateApplicationBuilder(args);

// MassTransit Yap?land?rmas?
builder.Services.AddMassTransit(x =>
{
    // 1. Bu servisin hangi Consumer'a sahip oldu?unu söylüyoruz
    x.AddConsumer<RentalCreatedConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        var rabbitMqHost = builder.Configuration["RabbitMQ:Host"];
        var rabbitMqUser = builder.Configuration["RabbitMQ:Username"];
        var rabbitMqPass = builder.Configuration["RabbitMQ:Password"];
        cfg.Host("localhost", "/", h =>
        {
            h.Username(rabbitMqUser);
            h.Password(rabbitMqPass);
        });

        // 2. RabbitMQ'da hangi kuyru?u (Queue) dinleyece?ini belirliyoruz
        cfg.ReceiveEndpoint("rental-created-queue", e =>
        {
            // Bu kuyru?a gelen mesajlar? ilgili Consumer'a yönlendir
            e.ConfigureConsumer<RentalCreatedConsumer>(context);
        });
    });
});

var host = builder.Build();
host.Run();