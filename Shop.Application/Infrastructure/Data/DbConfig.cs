namespace Shop.Application.Infrastructure.Data
{
    public class DbConfig : IConfig
    {
        public string Name => "Database";
        public string ConnectionString { get; set; }
    }
}
