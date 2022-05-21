using Shop.Application.Security;
using Shop.Application.Security.Services;

namespace Shop.Application.Security
{
    public static class Startup
    {
        public static IServiceCollection AddSecurityModuleService(this IServiceCollection services)
        {

            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IEncryptionService, EncryptionService>();

            return services;
        }
    }
}
