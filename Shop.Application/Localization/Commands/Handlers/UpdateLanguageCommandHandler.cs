using Shop.Application.Localization.Commands.Models;
using Shop.Application.Localization.Services;

namespace Shop.Application.Localization.Commands.Handlers
{
    public class UpdateLanguageCommandHandler : IRequestHandler<UpdateLanguageCommand, Response<int>>
    {
        private readonly IMediator _mediator;
        private readonly ShopDbContext _context;
        private readonly ILanguageService _languageService;
        private readonly ITranslationService _translationService;

        public UpdateLanguageCommandHandler(
            IMediator mediator, 
            ShopDbContext context, 
            ILanguageService languageService, 
            ITranslationService translationService)
        {
            _mediator = mediator;
            _context = context;
            _languageService = languageService;
            _translationService = translationService;
        }

        public async Task<Response<int>> Handle(UpdateLanguageCommand request, CancellationToken cancellationToken)
        {
            var language = await _languageService.GetLanguageByIdAsync(request.Id, tracked: true);

            if (language == null)
                throw new NotFoundException();

            if(!language.Code.EqualsNoCase(request.Code))
            {
                if(await _languageService.GetLanguageByCodeAsync(request.Code) != null)
                {
                    return Response<int>.Bad(await _translationService.GetTranslationAsync("Language.Error.AlreadyCodeExist"));
                }
            }

            if(language.IsActive && !request.IsActive)
            {
                var languages = await _languageService.GetLanguagesAsync(false);

                if(languages.Count == 1 && languages[0].Id == language.Id)
                {
                    return Response<int>.Bad(await _translationService.GetTranslationAsync("Language.Error.RequireAtLeastActive"));
                }
            }

            using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

            language.Name = request.Name;
            language.IsRtl = request.IsRtl;
            language.IsActive = request.IsActive;
            language.Code = request.Code.ToLower();
            language.Culture = request.Culture;
            language.DisplayOrder = request.DisplayOrder;

            if (request.CurrencyId == 0)
                language.CurrencyId = null;

            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            await _mediator.EntityUpdated(language);

            return Response<int>.Ok();
        }
    }
}
