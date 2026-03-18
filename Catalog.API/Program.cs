using Catalog.Application.Behaviors;
using Catalog.Application.Features.Vehicles.Commands.CreateVehicle;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((context, configuration) =>
{
    configuration
        .ReadFrom.Configuration(context.Configuration)
        .WriteTo.Console()
        .WriteTo.File("Logs/WanderSync-Log-.txt", rollingInterval: RollingInterval.Day)
        .WriteTo.Seq("http://localhost:5341")
        .Enrich.FromLogContext()
        .Enrich.WithMachineName();
});

// Add services to the container.
builder.Services.AddDbContext<Catalog.Infrastructure.Persistence.CatalogDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddMediatR(cfg =>
{
cfg.RegisterServicesFromAssembly(typeof(CreateVehicleCommand).Assembly);
cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(CachingBehavior<,>));

});

builder.Services.AddValidatorsFromAssembly(typeof(Catalog.Application.Features.Vehicles.Commands.CreateVehicle.CreateVehicleCommand).Assembly);
builder.Services.AddScoped<Catalog.Domain.Repositories.IVehicleRepository, Catalog.Infrastructure.Repositories.VehicleRepository>();
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6379";
    options.InstanceName = "WanderSync_Catalog_";
});
builder.Services.AddControllers();
builder.Services.AddScoped<Catalog.Application.Security.IJwtProvider, Catalog.Infrastructure.Security.JwtProvider>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]))
        };
    });
builder.Services.AddExceptionHandler<Catalog.API.Exceptions.GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Kimlik Do?rulama ba?l???. \r\n\r\n A?a??daki kutuya 'Bearer' yaz?p bo?luk b?rakt?ktan sonra Token'?n?z? yap??t?r?n.\r\n\r\n÷rnek: 'Bearer eyJhbGci...'",
        Name = "Authorization", 
        In = ParameterLocation.Header, 
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseSerilogRequestLogging();
app.UseExceptionHandler();
app.UseAuthentication();
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
