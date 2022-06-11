using Shop.Application.Catalog.Categories.Commands.Models;
using Shop.Application.Localization.Services;
using Shop.Application.Seo.Services;

namespace Shop.Application.Catalog.Categories.Commands.Handlers
{
    public class AddCategoryTranslationCommandHandler : IRequestHandler<AddCategoryTranslationCommand>
    {
        private readonly ShopDbContext _context;
        private readonly ISlugService _slugService;
        private readonly ITranslationEntityService _translationEntityService;

        public AddCategoryTranslationCommandHandler(
            ShopDbContext context, 
            ISlugService slugService,
            ITranslationEntityService translationEntityService)
        {
            _context = context;
            _slugService = slugService;
            _translationEntityService = translationEntityService;
        }

        public async Task<Unit> Handle(AddCategoryTranslationCommand request, CancellationToken cancellationToken)
        {
            Guard.IsNotNull(request, nameof(request));
            Guard.IsNotNull(request.Category, nameof(request.Category));
            Guard.IsNotNull(request.Language, nameof(request.Language));

            using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

            await _translationEntityService.SaveTranslationPropertyAsync(request.Category, p => p.Name, request.Name, request.Language.Id);
            await _translationEntityService.SaveTranslationPropertyAsync(request.Category, p => p.Description, request.Description, request.Language.Id);
            await _translationEntityService.SaveTranslationPropertyAsync(request.Category, p => p.ShortDescription, request.ShortDescription, request.Language.Id);
            await _translationEntityService.SaveTranslationPropertyAsync(request.Category, p => p.MetaTitle, request.MetaTitle, request.Language.Id);
            await _translationEntityService.SaveTranslationPropertyAsync(request.Category, p => p.MetaKeywords, request.MetaKeywords, request.Language.Id);
            await _translationEntityService.SaveTranslationPropertyAsync(request.Category, p => p.MetaDescription, request.MetaDescription, request.Language.Id);

            if (request.Slug.IsEmpty())
                request.Slug = await _slugService.ValidateSlugAsync(request.Category, request.Category.Name);
            else
                request.Slug = await _slugService.ValidateSlugAsync(request.Category, request.Slug);

            await _slugService.SaveSlugAsync(request.Category, request.Slug, request.Language.Id);

            await transaction.CommitAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
