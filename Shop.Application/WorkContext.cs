using Shop.Domain.Localization;
using Shop.Application.Localization.Services;

namespace Shop.Application
{
    public class WorkContext : IWorkContext
    {
        private readonly ILanguageService _languageService;
        public WorkContext(ILanguageService languageService)
        {
            _languageService = languageService;
        }

        public async Task<Language> GetWorkingLanguageAsync()
        {
            var language = await _languageService.GetLanguageByIdAsync(5);

            return language;
        }

        public Task SetWorkingLanguageAsync()
        {
            throw new NotImplementedException();
        }
    }
}
