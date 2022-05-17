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
                .AddLocalizationModuleService()
                .AddUsersModuleService();
              
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

        public static IServiceCollection AddLocalizationModuleService(this IServiceCollection services)
        {
            services.AddScoped<ILanguageService, LanguageService>();
            services.AddScoped<ILocalizationService, LocalizationService>();
            services.AddScoped<ILocalizedEntityService, LocalizedEntityService>();

            return services;
        }

        public static IServiceCollection AddUsersModuleService(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();

            return services;
        }
    }
}
