using Shop.Domain.Localization;

namespace Shop.Application.Localization.Services
{
    public class LocalizationService : ILocalizationService
    {
        private readonly IRepository<LocaleResource> _lrRepository;
        public LocalizationService(IRepository<LocaleResource> lrRepository)
        {
            _lrRepository = lrRepository;
        }

        public async Task<string> GetResourceAsync(string name, int languageId = 0)
        {
            var resource = await GetResourceByNameAsync(name, languageId);

            return resource.Name;
        }

        public async Task<LocaleResource> GetResourceByNameAsync(string name, int languageId = 0)
        {
            if (name.IsEmpty() || languageId < 0)
                return null;

            if (languageId == 0)
                languageId = 1;

            name = name.ToLower();

            var resource = await _lrRepository.Table.FirstOrDefaultAsync(p => p.Name == name && p.LanguageId == languageId);

            return resource;
        }

        
    }
}
