using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Store.G02.Domain.Contracts;
using Store.G02.Domain.Entities.Identity;
using Store.G02.Domain.Entities.Orders;
using Store.G02.Domain.Entities.Products;
using Store.G02.Persistence.Data.Contexts;
using Store.G02.Persistence.Identity.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Store.G02.Persistence
{
    public class DbInitializer(
        StoreDbContext _context, 
        IdentityStoreDbContext _identityContext,
        UserManager<AppUser> _userManager,
        RoleManager<IdentityRole> _roleManager) : IDbInitializer
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


            if (!_context.DeliveryMethods.Any())
            {
                var deliveryData = await File.ReadAllTextAsync(@"..\Infrastructure\Store.G02.Persistence\Data\DataSeeding\delivery.json");

                var deliveryMethod = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryData);

                if (deliveryMethod is not null && deliveryMethod.Count > 0)
                {
                    await _context.DeliveryMethods.AddRangeAsync(deliveryMethod);
                }
            }


            await _context.SaveChangesAsync();

        }

        public async Task InitializeIdentityAsync()
        {
            if (_identityContext.Database.GetPendingMigrationsAsync().GetAwaiter().GetResult().Any())
            {
                await _identityContext.Database.MigrateAsync();
            }


            // Data Seeding
            if (!_identityContext.Roles.Any())
            {
                await _roleManager.CreateAsync(new IdentityRole() { Name = "SuperAdmin" });
                await _roleManager.CreateAsync(new IdentityRole() { Name = "Admin" });
            }
              
            if(!_identityContext.Users.Any())
            {
                var superAdmin = new AppUser()
                {
                    UserName = "SuperAdmin",
                    DisplayName = "SuperAdmin",
                    Email = "SuperAdmin@gmail.com",
                    PhoneNumber = "01233345555",
                };

                var admin = new AppUser()
                {
                    UserName = "Admin",
                    DisplayName = "Admin",
                    Email = "Admin@gmail.com",
                    PhoneNumber = "01233345555",
                };

                await _userManager.CreateAsync(superAdmin, "P@ssW0rd");
                await _userManager.CreateAsync(admin, "P@ssW0rd");

                await _userManager.AddToRoleAsync(superAdmin, "SuperAdmin");
                await _userManager.AddToRoleAsync(admin, "Admin");


            }


        }
    }
}
