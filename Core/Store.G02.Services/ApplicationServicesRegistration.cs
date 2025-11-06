using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Store.G02.Services.Abstraction;
using Store.G02.Services.Mapping;
using Store.G02.Services.Mapping.Baskets;
using Store.G02.Services.Mapping.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.G02.Services
{
    public static class ApplicationServicesRegistration
    {

        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IServiceManager, ServiceManager>();
            services.AddAutoMapper(M => M.AddProfile(new ProductProfile(configuration)));
            services.AddAutoMapper(M => M.AddProfile(new BasketProfile()));
            services.AddAutoMapper(M => M.AddProfile(new OrderProfile()));

            return services;
        }

    }
}
