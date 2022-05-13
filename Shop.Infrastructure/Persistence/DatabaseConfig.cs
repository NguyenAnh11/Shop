using Shop.Application.Infrastructure.Configurations;

namespace Shop.Infrastructure.Persistence
{
    public class DatabaseConfig : IConfig
    {
        public string Name => "Database";
        public string ConnectionString { get; set; }
    }
}
