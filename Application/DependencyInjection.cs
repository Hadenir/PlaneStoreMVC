using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PlaneStore.Application.Services;

namespace PlaneStore.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IManufacturerService, ManufacturerService>();
            services.AddScoped<IAircraftService, AircraftService>();
            services.AddScoped<IOrderService, OrderService>();

            return services;
        }
    }
}
