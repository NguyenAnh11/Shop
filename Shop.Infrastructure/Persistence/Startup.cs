using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shop.Application.Infrastructure;
using Shop.Application.Infrastructure.Configurations;
using Shop.Application.Infrastructure.Persistence;
using Shop.Infrastructure.Persistence.Context;

namespace Shop.Infrastructure.Persistence
{
    public static class Startup
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services)
        {
            var connectionString = Singleton<AppConfig>.Instance.Get<DatabaseConfig>().ConnectionString;

            services.AddDbContext<ShopContext>(option => option.UseSqlServer(connectionString));

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            services.AddScoped(typeof(IReadRepository<>), typeof(Repository<>));

            return services;
        }
    }
}
