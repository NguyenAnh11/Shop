using Shop.Domain.Localization;

namespace Shop.Application.Localization.Services
{
    public interface ITranslationService : IAbstractService<TranslationResource>
    {
        Task<int> GetCountTranslationsByLanguageIdAsync(int languageId);

        Task<TranslationResource> GetTranslationByIdAsync(int id, bool tracked = false);

        Task<TranslationResource> GetTranslationByNameAsync(string name, int languageId, bool extractMatch = true);

        Task<string> GetTranslationAsync(string name);

        Task<string> GetTranslationAsync(string name, int langaugeId);

        Task<string> GetTranslationEnumAsync<TEnum>(TEnum enumValue) where TEnum : struct;

        Task<string> GetTranslationEnumAsync<TEnum>(TEnum enumValue, int languageId) where TEnum : struct;
    }
}
