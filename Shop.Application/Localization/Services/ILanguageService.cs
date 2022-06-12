using Shop.Domain.Localization;

namespace Shop.Application.Localization.Services
{
    public interface ILanguageService : IAbstractService<Language>
    {
        Task<Language> GetLanguageByCodeAsync(string code);

        Task<Language> GetLanguageByIdAsync(int id, bool tracked = false);

        Task<IList<Language>> GetLanguagesAsync(bool includeHidden = true, bool isRtl = false, bool tracked = false);
    }
}
