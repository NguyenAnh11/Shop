using Microsoft.Toolkit.Diagnostics;

namespace Shop.Application.Infrastructure.Configurations
{
    public class AppConfig
    {
        private readonly Dictionary<Type, IConfig> _configs = new();

        public AppConfig(IList<IConfig> configs)
        {
            Guard.IsNotNull(configs, nameof(configs));

            _configs = configs.ToDictionary(p => p.GetType(), p => p);
        }

        public TConfig Get<TConfig>() where TConfig : IConfig
        {
            if (_configs.TryGetValue(typeof(TConfig), out var obj) && obj is TConfig config)
                return config;

            return default;
        }

        public void Update(IList<IConfig> configs)
        {
            Guard.IsNotNull(configs, nameof(configs));

            foreach(var config in configs)
            {
                _configs[config.GetType()] = config;
            }
        }
    }
}
