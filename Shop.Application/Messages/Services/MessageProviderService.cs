using Shop.Application.Localization.Services;
using Shop.Application.Mailing;
using Shop.Domain.Localization;
using Shop.Domain.Users;

namespace Shop.Application.Messages.Services
{
    public class MessageProviderService : IMessageProviderService
    {
        private readonly IEmailService _emailService;
        private readonly IViewRenderService _viewRenderService;
        private readonly ITranslationService _translationService;

        public MessageProviderService(
            IEmailService emailService,
            IViewRenderService viewRenderService,
            ITranslationService translationService)
        {
            _emailService = emailService;
            _viewRenderService = viewRenderService;
            _translationService = translationService;
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

            await _emailService.SendAsync(new List<string> { user.Email }, await _translationService.GetTranslationAsync(subject, language.Id), body);
        }

        protected async Task SendMessageAsync(User user, Language language, string viewName, string subject)
        {
            Guard.IsNotNull(user, nameof(user));
            Guard.IsNotNull(language, nameof(language));
            Guard.IsNotNullOrEmpty(viewName, nameof(viewName));

            var viewPath = BuildViewPath(language.Code, viewName);

            var body = await _viewRenderService.RenderToStringAsync(viewPath);

            await _emailService.SendAsync(new List<string> { user.Email }, await _translationService.GetTranslationAsync(subject, language.Id), body);
        }

        public async Task SendActiveAccountMessageAsync(User user, Language language, string link)
            => await SendMessageAsync(user, language, link, "ActiveAccount", "Message.ActiveAccount");


        public async Task SendRecoveryPasswordMessageAsync(User user, Language language, string link)
            => await SendMessageAsync(user, language, link, "RecoveryPassword", "Message.RecoveryPassword");

        public async Task SendWelcomeMessageAsync(User user, Language language)
            => await SendMessageAsync(user, language, "Welcome", "Message.Welcome");
    }
}
