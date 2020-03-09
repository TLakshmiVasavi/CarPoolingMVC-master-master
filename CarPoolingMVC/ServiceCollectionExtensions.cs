using Microsoft.Extensions.DependencyInjection;
using Models.Interfaces;
using Services;

namespace CarPoolingMVC
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services)
        {
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<IRideService, RideService>();
            return services;
        }
    }
}
