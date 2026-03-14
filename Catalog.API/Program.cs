using Catalog.Application.Features.Vehicles.Commands.CreateVehicle;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<Catalog.Infrastructure.Persistence.CatalogDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateVehicleCommand).Assembly));
builder.Services.AddValidatorsFromAssembly(typeof(Catalog.Application.Features.Vehicles.Commands.CreateVehicle.CreateVehicleCommand).Assembly);

builder.Services.AddTransient(typeof(MediatR.IPipelineBehavior<,>), typeof(Catalog.Application.Behaviors.ValidationBehavior<,>));
builder.Services.AddScoped<Catalog.Domain.Repositories.IVehicleRepository, Catalog.Infrastructure.Repositories.VehicleRepository>();
builder.Services.AddControllers();
builder.Services.AddExceptionHandler<Catalog.API.Exceptions.GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseExceptionHandler();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<Catalog.Infrastructure.Persistence.CatalogDbContext>();

        context.Database.Migrate();

        await Catalog.Infrastructure.Persistence.CatalogDbContextSeed.SeedAsync(context);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Veritaban?na tohumlama yap?l?rken bir hata olu?tu: {ex.Message}");
    }
}
app.Run();
