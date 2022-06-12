using Shop.Application.Localization.Commands.Models;
using Shop.Application.Localization.Services;
using Shop.Domain.Localization;

namespace Shop.Application.Localization.Commands.Handlers
{
    public class CreateTranslationResourceCommandHandler : IRequestHandler<CreateTranslationResourceCommand, Response<int>>
    {
        private readonly IMediator _mediator;
        private readonly ShopDbContext _context;
        private readonly ITranslationService _translationService;

        public CreateTranslationResourceCommandHandler(
            IMediator mediator,
            ShopDbContext context, 
            ITranslationService translationService)
        {
            _mediator = mediator;
            _context = context;
            _translationService = translationService;
        }

        public async Task<Response<int>> Handle(CreateTranslationResourceCommand request, CancellationToken cancellationToken)
        {
            Guard.IsNotNull(request, nameof(request));

            var translation = await _translationService.GetTranslationByNameAsync(request.Name, request.LanguageId);

            if(translation != null)
            {
                return Response<int>.Bad(await _translationService.GetTranslationAsync("Resource.Error.NameAlreadyExist"));
            }

            using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

            translation = new TranslationResource
            {
                Value = request.Value,
                Name = request.Name.ToLower(),
                LanguageId = request.LanguageId
            };

            await _translationService.Table.AddAsync(translation, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            await _mediator.EntityInserted(translation);

            return Response<int>.Ok(translation.Id);
        }
    }
}
