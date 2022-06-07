using Shop.Domain.Configuration;
using System.Linq.Expressions;

namespace Shop.Application.Configurations.Services
{
    public interface ISettingService : IAbstractService<Setting>
    {
        Task<Setting> GetSettingByIdAsync(int id, bool tracked = false);

        Task<Setting> GetSettingByKeyAsync(string key, bool tracked = false);

        Task<T> GetSettingByKeyAsync<T>(string key);

        Task<T> LoadSettingAsync<T>() where T : ISetting, new();

        Task<ISetting> LoadSettingAsync(Type type);

        Task SaveSettingAsync<T>(T setting) where T : ISetting, new();

        Task SaveSettingAsync<T, TType>(T obj, Expression<Func<T, TType>> func) where T : ISetting, new();
    }
}
