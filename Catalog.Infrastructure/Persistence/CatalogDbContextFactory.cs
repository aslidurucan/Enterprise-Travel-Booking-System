using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Infrastructure.Persistence
{
    public class CatalogDbContextFactory : IDesignTimeDbContextFactory<CatalogDbContext>
    {
        public CatalogDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CatalogDbContext>();

            // Sadece Migration (göç) araçlarının kullanması için Docker veritabanımızın adresini buraya açıkça yazıyoruz:
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=WanderSyncCatalogDb;Username=postgres;Password=123456");

            return new CatalogDbContext(optionsBuilder.Options);
        }
    }
}
