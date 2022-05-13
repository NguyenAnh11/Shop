namespace Shop.Application.Infrastructure
{
    public static class Singleton<T>
    {
        private static readonly Dictionary<Type, object> _instance = new();

        public static T Instance
        {
            get
            {
                if (_instance.TryGetValue(typeof(T), out var t) && t is T instance)
                    return instance;

                return default;
            }
            set => _instance[typeof(T)] = value;
        }
    }
}
