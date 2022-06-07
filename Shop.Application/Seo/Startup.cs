using Shop.Application.Seo.Services;

namespace Shop.Application.Seo
{
    public static class Startup
    {
        public static IServiceCollection AddSeoModuleService(this IServiceCollection services)
        {
            services.AddScoped<ISlugService, SlugService>();

            return services;
        }
    }
}
