using Shop.Application.Authentication.Services;

namespace Shop.Application.Authentication
{
    public static class Startup
    {
        public static IServiceCollection AddAuthenticationModuleService(this IServiceCollection services)
        {
            services.AddScoped<IAuthenticationService, AuthenticationService>();

            return services;
        }
    }
}
