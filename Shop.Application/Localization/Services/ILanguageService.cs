using Shop.Domain.Localization;

namespace Shop.Application.Localization.Services
{
    public interface ILanguageService
    {
        Task<LanguageDto> GetLanguageByIdAsync(int id);
        Task<Language> GetLanguageByCodeAsync(string code);
        Task<IList<Language>> GetActiveLanguageAsync();
        Task<Response<Language>> InsertLanguageAsync(LanguageDto dto);
        Task<Response> UpdateLanguageAsync(int id, LanguageDto dto);
        Task<Response> DeleteLanguageAsync(int id);
    }
}
