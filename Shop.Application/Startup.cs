namespace Shop.Application
{
    public static class Startup
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services
                .AddAutoMapper()
                .AddSetting()
                .AddAppDbContext()
                .AddMediatorModuleService()
                .AddLocalizationModuleService()
                .AddSecurityModuleService()
                .AddUsersModuleService()
                .AddAuthenticationModuleService()
                .AddMessageModuleService()
                .AddSeoModuleService()
                .AddCatalogModuleService()
                .AddConfigurationModuleService()
                .AddMediaModuleService();

            services.AddScoped<IWorkContext, WorkContext>();
              
            return services;
        }

        public static IServiceCollection AddAutoMapper(this IServiceCollection services)
        {
            var profiles = Singleton<ITypeFinder>.Instance
                .FindClassOfType<IProfile>()
                .Select(p => (IProfile)Activator.CreateInstance(p))
                .ToList();

            var mapperConfiguration = new MapperConfiguration(config =>
            {
                foreach (var profile in profiles)
                {
                    config.AddProfile(profile.GetType());
                }
            });

            services.AddSingleton(mapperConfiguration.CreateMapper());

            return services;
        }

        public static IServiceCollection AddMediatorModuleService(this IServiceCollection services)
        {
            var assemlies = Singleton<ITypeFinder>.Instance.Assemblies();

            return services.AddMediatR(assemlies, config => config.AsTransient());
        }

        public static IServiceCollection AddSetting(this IServiceCollection services)
        {
            var settings = Singleton<ITypeFinder>.Instance
                .FindClassOfType<ISetting>()
                .Select(p => (ISetting)Activator.CreateInstance(p))
                .ToList();

            foreach (var setting in settings)
                services.AddSingleton(setting.GetType(), setting);

            return services;
        }

        public static IServiceCollection AddAppDbContext(this IServiceCollection services)
        {
            var connectionString = Singleton<AppConfig>.Instance.Get<DbConfig>().ConnectionString;

            services.AddDbContext<ShopDbContext>(p => p.UseSqlServer(connectionString));

            return services;
        }

        
    }
}
