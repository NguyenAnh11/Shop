using Shop.Application.Localization.Services;

namespace Shop.Application.Localization
{
    public static class Startup
    {
        public static IServiceCollection AddLocalizationModuleService(this IServiceCollection services)
        {
            services.AddScoped<ILanguageService, LanguageService>();
            services.AddScoped<ITranslationService, TranslationService>();
            services.AddScoped<ITranslationEntityService, TranslationEntityService>();

            return services;
        }
    }
}
