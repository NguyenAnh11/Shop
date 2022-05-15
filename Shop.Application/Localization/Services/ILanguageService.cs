namespace Shop.Application.Localization.Services
{
    public interface ILanguageService
    {
        Task<Language> GetLanguageByIdAsync(int id);
        Task<Language> GetLanguageByCodeAsync(string code);
        Task<IList<Language>> GetActiveLanguageAsync();
        Task<Response<Language>> InsertLanguageAsync(LanguageDto dto);
        Task<Response> UpdateLanguageAsync(int id, LanguageDto dto);
    }
}
