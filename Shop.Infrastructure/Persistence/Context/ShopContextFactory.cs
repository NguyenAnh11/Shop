using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Toolkit.Diagnostics;
using Shop.Application.Infrastructure;
using Shop.Application.Infrastructure.TypeFinder;

namespace Shop.Infrastructure.Persistence.Context
{
    public class ShopContextFactory : IDesignTimeDbContextFactory<ShopContext>
    {
        public ShopContext CreateDbContext(string[] args)
        {
            var enviroment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var basePath = AppDomain.CurrentDomain.BaseDirectory;

            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{enviroment}.json", false, true)
                .AddEnvironmentVariables()
                .Build();

            var typeFinder = new AppTypeFinder();
            Singleton<ITypeFinder>.Instance = typeFinder;

            var databaseConfig = new DatabaseConfig();
            configuration.GetSection(databaseConfig.Name).Bind(databaseConfig);

            Guard.IsNotEmpty(databaseConfig.ConnectionString, "ConnectionString");

            var assembly = typeof(ShopContext).Assembly.GetName().Name;

            var optionBuilder = new DbContextOptionsBuilder<ShopContext>();
            optionBuilder.UseSqlServer(databaseConfig.ConnectionString, option => option.MigrationsAssembly(assembly));

            return new ShopContext(optionBuilder.Options);
        }
    }
}
