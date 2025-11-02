using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Store.G02.Domain.Contracts;
using Store.G02.Domain.Entities.Identity;
using Store.G02.Persistence;
using Store.G02.Persistence.Identity.Contexts;
using Store.G02.Services;
using Store.G02.Shared.ErrorsModels;
using Store.G02.Web.Middlewares;
using System.Runtime.CompilerServices;

namespace Store.G02.Web.Extensions
{
    public static class Extensions
    {
        public static IServiceCollection RegisterAllServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddBuiltInServices();
            services.AddSwaggerServices();

            services.AddInfrastructureServices(configuration);
            services.AddApplicationServices(configuration);

            services.ConfigureServices();

            services.AddIdentityServices();
            

            return services;
        }

        private static IServiceCollection AddBuiltInServices(this IServiceCollection services)
        {
            services.AddControllers();
            return services;
        }

        private static IServiceCollection AddSwaggerServices(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            return services;
        }

        private static IServiceCollection AddIdentityServices(this IServiceCollection services)
        {
           services.AddIdentityCore<AppUser>(options =>
           {
               options.User.RequireUniqueEmail = true;
           }).AddRoles<IdentityRole>()
             .AddEntityFrameworkStores<IdentityStoreDbContext>();

            return services;
        }


        private static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(config =>
            {
                config.InvalidModelStateResponseFactory = (actioncontext) =>
                {
                    var errors = actioncontext.ModelState.Where(m => m.Value.Errors.Any())
                                 .Select(m => new ValidationError()
                                 {
                                     Field = m.Key,
                                     Errors = m.Value.Errors.Select(errors => errors.ErrorMessage)

                                 });

                    var response = new ValidationErrorResponse()
                    {
                        Errors = errors
                    };

                    return new BadRequestObjectResult(response);
                };
            });
            return services;
        }


        public static async Task<WebApplication> ConfigureMiddleWares(this WebApplication app)
        {
            await app.InitializeDatabaseAsync();

            app.UseGlobalErrorHandling();

            app.UseStaticFiles();


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            return app;
        }


        private static async Task<WebApplication> InitializeDatabaseAsync(this WebApplication app)
        {
            #region Initialize Db
            using var scope = app.Services.CreateScope();
            var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();  // Ask CLR To Create Object From IDbInitializer 
            await dbInitializer.InitializeAsync();
            await dbInitializer.InitializeIdentityAsync();
            #endregion

            return app;

        }

        private static WebApplication UseGlobalErrorHandling(this WebApplication app)
        {
            app.UseMiddleware<GlobalErrorHandlingMiddleware>();

            return app;
        }





    }

}
