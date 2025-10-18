using Microsoft.EntityFrameworkCore;
using Store.G02.Domain.Contracts;
using Store.G02.Domain.Entities.Products;
using Store.G02.Persistence.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Store.G02.Persistence
{
    public class DbInitializer(StoreDbContext _context) : IDbInitializer
    {       
        public async Task InitializeAsync()
        {
            // Create Db

            // Update Db
            if (_context.Database.GetPendingMigrationsAsync().GetAwaiter().GetResult().Any())
            {
                await _context.Database.MigrateAsync();
            }


            // Data Seeding

            // ProductBrands

            if(!_context.ProductBrands.Any())
            {
                // 1. Read All Data From Json File 'brands.json' 
                var brandData = await File.ReadAllTextAsync(@"..\Infrastructure\Store.G02.Persistence\Data\DataSeeding\brands.json");

                // 2. Convert JasonString To List<ProductBrand>
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandData);

                // 3. Add List To The Db
                if (brands is not null && brands.Count > 0)
                {
                    await _context.ProductBrands.AddRangeAsync(brands);
                }
            }


            // ProductTypes

            if (!_context.ProductTypes.Any())
            {
                var typeData = await File.ReadAllTextAsync(@"..\Infrastructure\Store.G02.Persistence\Data\DataSeeding\types.json");

                var types = JsonSerializer.Deserialize<List<ProductType>>(typeData);

                if (types is not null && types.Count > 0)
                {
                    await _context.ProductTypes.AddRangeAsync(types);
                }
            }


            // Product

            if(!_context.Products.Any())
            {
                var productData = await File.ReadAllTextAsync(@"..\Infrastructure\Store.G02.Persistence\Data\DataSeeding\products.json");

                var products = JsonSerializer.Deserialize<List<Product>>(productData);

                if(products is  not null && products.Count > 0)
                {
                    await _context.Products.AddRangeAsync(products);
                }
            }


            await _context.SaveChangesAsync();

        }

    }
}
