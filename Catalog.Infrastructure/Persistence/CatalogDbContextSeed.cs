using Bogus;
using Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Infrastructure.Persistence
{
    public static class CatalogDbContextSeed
    {
        public static async Task SeedAsync(CatalogDbContext context)
        {
            
            if (!await context.Vehicles.AnyAsync())
            {
               
                var vehicleFaker = new Faker<Vehicle>("tr")
                    .RuleFor(v => v.Id, f => Guid.NewGuid()) 
                    .RuleFor(v => v.Brand, f => f.Vehicle.Manufacturer()) 
                    .RuleFor(v => v.Model, f => f.Vehicle.Model()) 
                    .RuleFor(v => v.Year, f => f.Random.Int(2015, 2024)) 
                    .RuleFor(v => v.DailyPrice, f => f.Random.Decimal(1500, 8000)) 
                    .RuleFor(v => v.Currency, "TRY"); 

                var fakeVehicles = vehicleFaker.Generate(1000);

                await context.Vehicles.AddRangeAsync(fakeVehicles);

                await context.SaveChangesAsync();
            }
        }
    }
}
