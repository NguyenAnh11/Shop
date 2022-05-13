using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shop.Application.Infrastructure;
using Shop.Application.Infrastructure.Configurations;
using Shop.Application.Infrastructure.TypeFinder;
using Shop.Infrastructure.Persistence;

namespace Shop.Infrastructure
{
    public static class Startup
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var typeFinder = new AppTypeFinder();
            Singleton<ITypeFinder>.Instance = typeFinder;

            var configs = typeFinder
                .FindClassOfType<IConfig>()
                .Select(p => (IConfig)Activator.CreateInstance(p))
                .ToList();

            foreach (var config in configs)
                configuration.GetSection(config.Name).Bind(config);

            var appConfig = new AppConfig(configs);
            Singleton<AppConfig>.Instance = appConfig;

            services.AddPersistence();

            return services;
        }

        public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            return app;
        }
    }
}
