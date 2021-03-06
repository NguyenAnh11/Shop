using Shop.Application.Messages.Services;

namespace Shop.Application.Messages
{
    public static class Startup
    {
        public static IServiceCollection AddMessageModuleService(this IServiceCollection services)
        {
            services.AddScoped<IMessageProviderService, MessageProviderService>();
            services.AddScoped<IViewRenderService, ViewRenderService>();

            return services;
        }
    }
}
