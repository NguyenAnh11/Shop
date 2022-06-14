namespace Shop.Application.Infrastructure.Caching
{
    public class CacheConfig : IConfig
    {
        public string Name => "Cache";

        public int ShortCacheTime { get; set; } = 3; //3 mintues
        public int DefaultCacheTime { get; set; } = 30; //30 minutes
        public int LongCacheTime { get; set; } = 30 * 24 * 60; //30 day

    }
}
