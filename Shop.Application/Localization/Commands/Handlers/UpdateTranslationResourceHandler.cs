using Shop.Application.Localization.Commands.Models;
using Shop.Application.Localization.Services;

namespace Shop.Application.Localization.Commands.Handlers
{
    public class UpdateTranslationResourceHandler : IRequestHandler<UpdateTranslationResourceCommand, Response<int>>
    {
        private readonly IMediator _mediator;
        private readonly ShopDbContext _context;
        private readonly ITranslationService _translationService;

        public UpdateTranslationResourceHandler(
            IMediator mediator,
            ShopDbContext context,
            ITranslationService translationService)
        {
            _mediator = mediator;
            _context = context;
            _translationService = translationService;
        }

        public async Task<Response<int>> Handle(UpdateTranslationResourceCommand request, CancellationToken cancellationToken)
        {
            Guard.IsNotNull(request, nameof(request));

            var translation = await _translationService.GetTranslationByIdAsync(request.Id, tracked: true);

            if (translation == null)
                throw new NotFoundException();

            if (!translation.Name.EqualsNoCase(request.Name))
            {
                if (await _translationService.GetTranslationByNameAsync(request.Name, request.LanguageId) != null)
                {
                    return Response<int>.Bad(await _translationService.GetTranslationAsync("Resource.Error.NameAlreadyExist"));
                }
            }

            using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

            translation.Value = request.Value;
            translation.Name = request.Name.ToLower();
            translation.LanguageId = request.LanguageId;

            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            await _mediator.EntityUpdated(translation);

            return Response<int>.Ok();
        }
    }
}
