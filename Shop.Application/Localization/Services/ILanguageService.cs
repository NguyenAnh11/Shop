using Shop.Application.Localization.Dtos;
using Shop.Domain.Localization;

namespace Shop.Application.Localization.Services
{
    public interface ILanguageService : IAbstractService<Language>
    {
        Task<Language> GetLanguageByCodeAsync(string code);

        Task<Language> GetLanguageByIdAsync(int id, bool tracked = false);

        Task<IList<Language>> GetLanguagesAsync(bool includeHidden = true, bool isRtl = false, bool tracked = false);

        Task<Response<int>> InsertLanguageAsync(LanguageDto dto);

        Task<Response> UpdateLanguageAsync(LanguageDto dto);

        Task<Response> DeleteLanguageAsync(Language language);
    }
}                             
