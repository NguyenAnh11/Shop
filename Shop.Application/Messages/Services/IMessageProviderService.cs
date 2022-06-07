using Shop.Domain.Users;
using Shop.Domain.Localization;

namespace Shop.Application.Messages.Services
{
    public interface IMessageProviderService
    {
        Task SendWelcomeMessageAsync(User user, Language language);

        Task SendActiveAccountMessageAsync(User user, Language language, string link); 

        Task SendRecoveryPasswordMessageAsync(User user, Language language, string link);
    }
}
