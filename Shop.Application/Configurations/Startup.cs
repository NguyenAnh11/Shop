using Shop.Application.Configurations.Services;

namespace Shop.Application.Configurations
{
    public static class Startup
    {
        public static IServiceCollection AddConfigurationModuleService(this IServiceCollection services)
        {
            services.AddScoped<ISettingService, SettingService>();

            return services;
        }
    }
}
