using Shop.Application.Media.Services;

namespace Shop.Application.Media
{
    public static class Startup
    {
        public static IServiceCollection AddMediaModuleService(this IServiceCollection services)
        {
            services.AddScoped<IPictureService, PictureService>();
            return services;
        }
    }
}
