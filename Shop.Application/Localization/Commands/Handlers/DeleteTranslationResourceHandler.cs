using Shop.Application.Localization.Commands.Models;
using Shop.Application.Localization.Services;

namespace Shop.Application.Localization.Commands.Handlers
{
    public class DeleteTranslationResourceHandler : IRequestHandler<DeleteTranslationResourceCommand>
    {
        private readonly IMediator _mediator;
        private readonly ShopDbContext _context;
        private readonly ITranslationService _translationService;

        public DeleteTranslationResourceHandler(
            IMediator mediator,
            ShopDbContext context,
            ITranslationService translationService)
        {
            _mediator = mediator;
            _context = context;
            _translationService = translationService;
        }

        public async Task<Unit> Handle(DeleteTranslationResourceCommand request, CancellationToken cancellationToken)
        {
            Guard.IsNotNull(request, nameof(request.Translation));

            using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

            _translationService.Table.Remove(request.Translation);

            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            await _mediator.EntityDeleted(request.Translation);

            return Unit.Value;
        }
    }
}
