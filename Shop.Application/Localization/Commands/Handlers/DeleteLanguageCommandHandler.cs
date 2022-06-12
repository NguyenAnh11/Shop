using Shop.Application.Localization.Commands.Models;
using Shop.Application.Localization.Services;
using Shop.Application.Localization.Settings;
using Shop.Application.Configurations.Services;
using Shop.Domain.Localization;

namespace Shop.Application.Localization.Commands.Handlers
{
    public class DeleteLanguageCommandHandler : IRequestHandler<DeleteLanguageCommand, Response>
    {
        private readonly IMediator _mediator;
        private readonly ShopDbContext _context;
        private readonly ISettingService _settingService;
        private readonly ILanguageService _languageService;
        private readonly ITranslationService _translationService;
        private readonly LocalizationSetting _localizationSetting;

        public DeleteLanguageCommandHandler(
            IMediator mediator,
            ShopDbContext context, 
            ISettingService settingService,
            ILanguageService languageService,
            ITranslationService translationService,
            LocalizationSetting localizationSetting)
        {
            _mediator = mediator;
            _context = context;
            _settingService = settingService;
            _languageService = languageService;
            _translationService = translationService;
            _localizationSetting = localizationSetting;
        }

        public async Task<Response> Handle(DeleteLanguageCommand request, CancellationToken cancellationToken)
        {
            Guard.IsNotNull(request.Language, nameof(request.Language));

            using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

            if(request.Language.IsActive)
            {
                var languages = await _languageService.GetLanguagesAsync(false, tracked: true);

                if(languages.Count == 1 && languages[0].Id == request.Language.Id)
                {
                    return Response.Bad(await _translationService.GetTranslationAsync("Language.Error.RequireAtLeastActive"));
                }

                if(_localizationSetting.DefaultLanguageAdminId == request.Language.Id)
                {
                    _localizationSetting.DefaultLanguageAdminId = request.Language.Id;
                    await _settingService.SaveSettingAsync(_localizationSetting, p => p.DefaultLanguageAdminId);
                }
            }

            _languageService.Table.Remove(request.Language);

            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            await _mediator.EntityDeleted(request.Language);

            return Response.Ok();
        }
    }
}
