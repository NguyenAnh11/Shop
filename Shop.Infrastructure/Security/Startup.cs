using Shop.Application.Security.Services;
using Shop.Infrastructure.Security.Services;

namespace Shop.Infrastructure.Security
{
    public static class Startup
    {
        public static IServiceCollection AddSecurityService(this IServiceCollection services)
        {
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IEncryptionService, EncryptionService>();

            return services;
        }
    }
}
