using Shop.Application.Localization.Services;
using Shop.Application.Mailing;
using Shop.Domain.Localization;
using Shop.Domain.Users;

namespace Shop.Application.Messages.Services
{
    public class MessageService : IMessageService
    {
        private readonly IEmailService _emailService;
        private readonly IViewRenderService _viewRenderService;
        private readonly ILocalizationService _localizationService;

        public MessageService(
            IEmailService emailService,
            IViewRenderService viewRenderService,
            ILocalizationService localizationService)
        {
            _emailService = emailService;
            _viewRenderService = viewRenderService;
            _localizationService = localizationService;
        }

        protected static string BuildViewPath(string code, string viewName)
            => @$"/Views/{code}/{viewName}.cshtml";

        protected async Task SendMessageAsync<T>(User user, Language language, T model, string viewName, string subject)
        {
            Guard.IsNotNull(user, nameof(user));
            Guard.IsNotNull(language, nameof(language));
            Guard.IsNotNullOrEmpty(viewName, nameof(viewName));

            var viewPath = BuildViewPath(language.Code, viewName);

            var body = await _viewRenderService.RenderToStringAsync(viewPath, model);

            await _emailService.SendEmailAsync(new List<string> { user.Email }, await _localizationService.GetResourceAsync(subject), body);
        }

        protected async Task SendMessageAsync(User user, Language language, string viewName, string subject)
        {
            Guard.IsNotNull(user, nameof(user));
            Guard.IsNotNull(language, nameof(language));
            Guard.IsNotNullOrEmpty(viewName, nameof(viewName));

            var viewPath = BuildViewPath(language.Code, viewName);

            var body = await _viewRenderService.RenderToStringAsync(viewPath);

            await _emailService.SendEmailAsync(new List<string> { user.Email }, await _localizationService.GetResourceAsync(subject), body);
        }

        public async Task SendActiveAccountMessageAsync(User user, Language language, string link)
            => await SendMessageAsync(user, language, link, SystemMessageName.ActiveAccount, "Message.ActiveAccount");


        public async Task SendRecoveryPasswordMessageAsync(User user, Language language, string link)
            => await SendMessageAsync(user, language, link, SystemMessageName.RecoveryPassword, "Message.RecoveryPassword");

        public async Task SendWelcomeMessageAsync(User user, Language language)
            => await SendMessageAsync(user, language, SystemMessageName.Welcome, "Message.Welcome");
    }
}
