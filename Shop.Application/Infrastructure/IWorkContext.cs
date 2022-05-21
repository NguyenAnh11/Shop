using Shop.Domain.Localization;

namespace Shop.Application.Infrastructure
{
    public interface IWorkContext
    {
        Task<Language> GetWorkingLanguageAsync();

        Task SetWorkingLanguageAsync();
    }
}
