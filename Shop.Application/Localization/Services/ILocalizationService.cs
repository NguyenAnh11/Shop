using Shop.Domain.Localization; 

namespace Shop.Application.Localization.Services
{
    public interface ILocalizationService 
    {
        Task<string> GetResourceAsync(string name, int languageId = 0);
        Task<LocaleResource> GetResourceByNameAsync(string name, int languageId = 0); 
    }
}
