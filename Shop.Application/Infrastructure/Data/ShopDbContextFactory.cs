using Microsoft.EntityFrameworkCore.Design;

namespace Shop.Application.Infrastructure.Data
{
    public class ShopDbContextFactory : IDesignTimeDbContextFactory<ShopDbContext>
    {
        public ShopDbContext CreateDbContext(string[] args)
        {
            var enviroment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var basePath = AppDomain.CurrentDomain.BaseDirectory;

            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{enviroment}.json", true, true)
                .AddEnvironmentVariables()
                .Build();

            var typeFinder = new AppTypeFinder();
            Singleton<ITypeFinder>.Instance = typeFinder;

            var dbConfig = new DbConfig();
            configuration.GetSection(dbConfig.Name).Bind(dbConfig);

            var migrationAssembly = typeof(ShopDbContext).Assembly.GetName().Name;

            var optionBuilder = new DbContextOptionsBuilder<ShopDbContext>();
            optionBuilder.UseSqlServer(dbConfig.ConnectionString, option => option.MigrationsAssembly(migrationAssembly));

            return new ShopDbContext(optionBuilder.Options);
        }
    }
}
