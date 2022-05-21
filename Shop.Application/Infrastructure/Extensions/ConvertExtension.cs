using System.ComponentModel;

namespace Shop.Application.Infrastructure.Extensions
{
    public static class ConvertExtension
    {
        public static T ChangeType<T>(this object value)
        {
            if (value is T variable) return variable;

            try
            {
                if (Nullable.GetUnderlyingType(typeof(T)) != null)
                {
                    return (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFrom(value);
                }

                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch
            {
                return default;
            }
        }
    }
}
