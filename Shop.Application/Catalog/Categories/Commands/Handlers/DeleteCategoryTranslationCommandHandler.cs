using Shop.Application.Catalog.Categories.Commands.Models;
using Shop.Application.Localization.Services;
using Shop.Application.Seo.Services;

namespace Shop.Application.Catalog.Categories.Commands.Handlers
{
    public class DeleteCategoryTranslationCommandHandler : IRequestHandler<DeleteCategoryTranslationCommand, Unit>
    {
        private readonly ShopDbContext _context;
        private readonly ISlugService _slugService;
        private readonly ITranslationEntityService _translationEntityService;

        public DeleteCategoryTranslationCommandHandler(
            ShopDbContext context, 
            ISlugService slugService,
            ITranslationEntityService translationEntityService)
        {
            _context = context;
            _slugService = slugService;
            _translationEntityService = translationEntityService;
        }

        public async Task<Unit> Handle(DeleteCategoryTranslationCommand request, CancellationToken cancellationToken)
        {
            Guard.IsNotNull(request, nameof(request.Category)); 
            Guard.IsNotNull(request.Language, nameof(request.Language));

            using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

            await _translationEntityService.DeleteTranslationEntityByLanguageAsync(request.Category, request.Language.Id);

            await _slugService.InActiveSlugAsync(request.Category, request.Language.Id);

            await transaction.CommitAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
