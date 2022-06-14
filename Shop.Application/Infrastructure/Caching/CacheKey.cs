namespace Shop.Application.Infrastructure.Caching
{
    public class CacheKey
    {
        public string Key { get; set; }
        public int CacheTime { get; set; } = 0;
        public IList<string> Prefixes { get; set; } = new List<string>();

        public CacheKey(string key, IList<string> prefixes)
        {
            Guard.IsNotNull(key, nameof(key));
            Key = key;

            CacheTime = 0;

            if(prefixes != null)
                Prefixes = prefixes.Where(p => !p.IsEmpty()).ToList();
        }

        public CacheKey(string key, int cacheTime, IList<string> prefixes)
        {
            Guard.IsNotNull(key, nameof(key));
            Key = key;

            CacheTime = cacheTime;
            if (prefixes != null)
                Prefixes = prefixes.Where(p => !p.IsEmpty()).ToList();
        }

        public CacheKey Create(Func<object, object> createKeyParameters, params object[] parameters)
        {
            if (!parameters.Any())
                return this;

            this.Key = string.Format(this.Key, parameters.Select(parameter => createKeyParameters(parameter)));



            return null;
        }
    }
}
