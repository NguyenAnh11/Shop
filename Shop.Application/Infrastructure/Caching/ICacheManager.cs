namespace Shop.Application.Infrastructure.Caching
{
    public interface ICacheManager
    {
        Task<T> GetAsync<T>(string key, Func<Task<T>> accquire);
        Task<T> GetAsync<T>(string key, Func<T> accquire);
        
    }
}
