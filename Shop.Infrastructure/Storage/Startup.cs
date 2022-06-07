using Shop.Infrastructure.Storage.Local;

namespace Shop.Infrastructure.Storage
{
    public static class Startup
    {
        public static IServiceCollection AddStorageModuleService(this IServiceCollection services)
        {
            services.AddSingleton<IStorageService, LocalStorageService>();
            return services;
        }
    }
}
