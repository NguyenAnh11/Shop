using Shop.Application.Localization.Dtos;
using Shop.Domain.Localization;

namespace Shop.Application.Localization.Services
{
    public interface ITranslationService : IAbstractService<TranslationResource>
    {
        Task<TranslationResource> GetResourceByIdAsync(int id);

        Task<TranslationResource> GetResourceByNameAsync(string name, int languageId, bool extractMatch = true);

        Task<string> GetResourceAsync(string name);

        Task<string> GetResourceAsync(string name, int langaugeId);

        Task<string> GetLocalizedEnumAsync<TEnum>(TEnum enumValue) where TEnum : struct;

        Task<string> GetLocalizedEnumAsync<TEnum>(TEnum enumValue, int languageId) where TEnum : struct;

        Task<Response<int>> InsertResourceAsync(LocaleResourceDto dto);

        Task<Response> UpdateResourceAsync(LocaleResourceDto dto);

        Task<Response> DeleteResourceAsync(TranslationResource resource);
    }
}
