using App.Errors;
using App.Helpers;
using App.Managers;
using App.Managers.Contract;
using App.Repositories;
using App.Repositories.Base;
using App.Repositories.Common.Service.Contract;
using App.Repositories.Context;
using App.Repositories.Contract;
using App.Repositories.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Extension
{
    public static class ApplicationServicesExtension
    {

        public static IServiceCollection AddApplicationServices(this IServiceCollection services,
          IConfiguration config, IWebHostEnvironment env)
        {


            services.AddHttpContextAccessor();

            // Register AutoMapper
            services.AddAutoMapper(typeof(MappingProfile));

            // Register Common Services
            services.AddScoped<ICurrentUserService, CurrentUserService>();

            // services
            services.AddScoped<ICustomerService, CustomerService>();


            services.AddDbContext<StoreContext>(options =>
            options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState
                        .Where(e => e.Value.Errors.Count > 0)
                        .SelectMany(x => x.Value.Errors)
                        .Select(x => x.ErrorMessage).ToArray();

                    var errorResponse = new ApiValidationErrorResponse
                    {
                        Errors = errors
                    };

                    return new BadRequestObjectResult(errorResponse);
                };
            });

            services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200");
                });
            });

            return services;
        }
    }
}
