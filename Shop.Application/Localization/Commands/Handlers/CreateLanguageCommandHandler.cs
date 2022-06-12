using Shop.Application.Localization.Commands.Models;
using Shop.Application.Localization.Services;
using Shop.Domain.Localization;

namespace Shop.Application.Localization.Commands.Handlers
{
    public class CreateLanguageCommandHandler : IRequestHandler<CreateLanguageCommand, Response<int>>
    {
        private readonly IMediator _mediator;
        private readonly ShopDbContext _context;
        private readonly ILanguageService _languageService;
        private readonly ITranslationService _translationService;

        public CreateLanguageCommandHandler(
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

        public async Task<Response<int>> Handle(CreateLanguageCommand request, CancellationToken cancellationToken)
        {
            Guard.IsNotNull(request, nameof(request));

            var language = await _languageService.GetLanguageByCodeAsync(request.Code);

            if (language != null)
            {
                return Response<int>.Bad(await _translationService.GetTranslationAsync("Language.Error.AlreadyCodeExist"));
            }

            using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

            language = new Language
            {
                Name = request.Name,
                IsRtl = request.IsRtl,
                IsActive = request.IsActive,
                Code = request.Code.ToLower(),
                Culture = request.Culture,
                DisplayOrder = request.DisplayOrder
            };

            if (request.CurrencyId == 0)
                language.CurrencyId = null;

            await _languageService.Table.AddAsync(language, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            await _mediator.EntityInserted(language);

            return Response<int>.Ok(language.Id);
        }
    }
}
