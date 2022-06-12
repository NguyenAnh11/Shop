using Shop.Application.Catalog.Categories.Services;

namespace Shop.Application.Catalog
{
    public static class Startup
    {
        public static IServiceCollection AddCatalogModuleService(this IServiceCollection services)
        {
            services.AddScoped<ICategoryService, CategoryService>();

            return services;
        }
    }
}
