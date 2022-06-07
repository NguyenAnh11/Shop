using Shop.Domain.Configuration;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace Shop.Application.Configurations.Services
{
    public class SettingService : AbstractService<Setting>, ISettingService
    {
        public SettingService(ShopDbContext context) : base(context)
        {
        }

        public async Task<Setting> GetSettingByIdAsync(int id, bool tracked = false)
            => await Table.FindByIdAsync(id, tracked);

        public async Task<Setting> GetSettingByKeyAsync(string key, bool tracked = false)
        {
            if (key.IsEmpty())
                return null;

            var setting = await Table
                .ApplyTracking(tracked)
                .FirstOrDefaultAsync(p => p.Key == key.ToLower());

            return setting;
        }

        public async Task<T> GetSettingByKeyAsync<T>(string key)
        {
            var setting = await GetSettingByKeyAsync(key);

            if (setting == null)
                return default;

            if (!TypeDescriptor.GetConverter(typeof(T)).CanConvertFrom(typeof(string)))
                return default;

            if (!TypeDescriptor.GetConverter(typeof(T)).IsValid(setting.Value))
                return default;

            var value = TypeDescriptor.GetConverter(typeof(T)).ConvertFromInvariantString(setting.Value);

            return (T)value;
        }

        public async Task<T> LoadSettingAsync<T>() where T : ISetting, new()
            => (T)await LoadSettingAsync(typeof(T));

        public async Task<ISetting> LoadSettingAsync(Type type)
        {
            Guard.IsNotNull(type, nameof(type));

            var obj = Activator.CreateInstance(type);

            foreach (var prop in type.GetProperties())
            {
                if (!prop.CanRead || !prop.CanWrite)
                    continue;

                if (!TypeDescriptor.GetConverter(prop.PropertyType).CanConvertFrom(typeof(string)))
                    continue;

                var key = string.Join('.', type.Name, prop.Name);

                var setting = await GetSettingByKeyAsync(key);

                if (!TypeDescriptor.GetConverter(prop.PropertyType).IsValid(setting.Value))
                    continue;

                var value = TypeDescriptor.GetConverter(prop.PropertyType).ConvertFromInvariantString(setting.Value);

                prop.SetValue(obj, value);
            }

            return (ISetting)obj;
        }

        private static PropertyInfo GetPropertyInfo<T, TType>(Expression<Func<T, TType>> func)
        {
            var body = func.Body as MemberExpression;

            Guard.IsNotOfType(func.Body, typeof(MemberExpression), nameof(func.Body));

            var propInfo = (PropertyInfo)body.Member;

            Guard.IsNotOfType(propInfo, typeof(PropertyInfo), nameof(propInfo));

            return propInfo;
        }

        public async Task SaveSettingAsync<T>(T obj) where T : ISetting, new()
        {
            var type = typeof(T);

            using var transaction = await _context.Database.BeginTransactionAsync();

            foreach(var property in type.GetProperties())
            {
                if (!property.CanWrite || !property.CanRead)
                    continue;

                if (!TypeDescriptor.GetConverter(property.PropertyType).CanConvertFrom(typeof(string)))
                    continue;

                var key = string.Join('.', type.Name, property.Name);

                var value = TypeDescriptor.GetConverter(property.PropertyType).ConvertToInvariantString(property.GetValue(obj));

                var setting = await GetSettingByKeyAsync(key, tracked: true);

                if (setting != null)
                    setting.Value = value;
                else
                {
                    setting = new Setting
                    {
                        Key = key,
                        Value = value
                    };

                    await Table.AddAsync(setting);
                }
            }

            await _context.SaveChangesAsync();

            await transaction.CommitAsync();
        }

        public async Task SaveSettingAsync<T, TType>(T obj, Expression<Func<T, TType>> func)
            where T : ISetting, new()
        {
            Guard.IsNotNull(func, nameof(func));

            var propInfo = GetPropertyInfo(func);

            if (!propInfo.CanWrite || !propInfo.CanRead)
                return;

            var key = string.Join('.', typeof(T).Name, propInfo.Name);

            if (!TypeDescriptor.GetConverter(propInfo.PropertyType).CanConvertFrom(typeof(string)))
                return;

            var value = TypeDescriptor.GetConverter(propInfo.PropertyType).ConvertToInvariantString(propInfo.GetValue(obj));

            var setting = await GetSettingByKeyAsync(key, true);

            if (setting != null)
                setting.Value = value;
            else
            {
                setting = new Setting
                {
                    Key = key,
                    Value = value
                };

                await Table.AddAsync(setting);
            }

            await _context.SaveChangesAsync();
        }
    }
}