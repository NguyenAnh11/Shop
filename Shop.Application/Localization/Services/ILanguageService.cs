using Shop.Application.Localization.Dtos;
using Shop.Domain.Localization;

namespace Shop.Application.Localization.Services
{
    public interface ILanguageService : IAbstractService<Language>
    {
        Task<IList<Language>> GetLanguagesAsync(bool includeHidden = true, bool isRtl = false);

        Task<Language> GetLanguageByCodeAsync(string code);

        Task<LanguageDto> GetLanguageByIdAsync(int id);

        Task<Response<int>> InsertLanguageAsync(LanguageDto dto);

        Task<Response> UpdateLanguageAsync(LanguageDto dto);

        Task<Response> DeleteLanguageAsync(int id);
    }
}                             
