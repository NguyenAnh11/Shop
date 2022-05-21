namespace Shop.Infrastructure.Cors
{
    public class CorsConfig : IConfig
    {
        public string Name => "Cors";
        public IList<string> CorsOrigins { get; set; } = new List<string>()
        {
            "http://localhost:3000"
        };
    }
}
