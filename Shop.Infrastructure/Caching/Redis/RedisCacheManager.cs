namespace Shop.Infrastructure.Caching.Redis
{
    public class RedisCacheManager : ICacheManager
    {
        public Task<T> GetAsync<T>(string key, Func<Task<T>> accquire)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetAsync<T>(string key, Func<T> accquire)
        {
            throw new NotImplementedException();
        }
    }
}
