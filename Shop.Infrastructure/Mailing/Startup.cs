using Shop.Application.Mailing;
using Shop.Infrastructure.Mailing.Stmp;

namespace Shop.Infrastructure.Mailing
{
    public static class Startup
    {
        public static IServiceCollection AddMailingModuleService(this IServiceCollection services)
        {

            services.AddSingleton<IEmailService, StmpEmailService>();

            return services;
        }
    }
}
