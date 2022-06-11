using Shop.Application.Catalog.Categories.Commands.Models;
using Shop.Application.Catalog.Categories.Services;
using Shop.Application.Seo.Services;
using Shop.Domain.Catalog;

namespace Shop.Application.Catalog.Categories.Commands.Handlers
{
    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand>
    {
        private readonly IMediator _meidator;
        private readonly ShopDbContext _context;
        private readonly ISlugService _slugService;
        private readonly ICategoryService _categoryService;

        public DeleteCategoryCommandHandler(
            IMediator mediator,
            ShopDbContext context, 
            ISlugService slugService, 
            ICategoryService categoryService)
        {
            _meidator = mediator;
            _context = context;
            _slugService = slugService;
            _categoryService = categoryService;
        }

        public async Task<Unit> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            Guard.IsNotNull(request.Category, nameof(request.Category));

            using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

            _context.Set<Category>().Remove(request.Category);

            var subCategories = await _categoryService.GetCategoriesByParentCategoryIdAsync(request.Category.Id);

            if(subCategories.Any())
            {
                foreach(var subCategory in subCategories)
                {
                    subCategory.ParentCategoryId = null;
                }
            }

            await _context.SaveChangesAsync(cancellationToken);

            await _slugService.DeleteSlugAsync(request.Category);

            await transaction.CommitAsync(cancellationToken);

            await _meidator.EntityDeleted(request.Category);

            return Unit.Value;
        }
    }
}
