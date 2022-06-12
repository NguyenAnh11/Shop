using Shop.Application.Catalog.Categories.Commands.Models;
using Shop.Application.Catalog.Categories.Services;
using Shop.Application.Localization.Services;
using Shop.Application.Media.Services;
using Shop.Application.Seo.Services;

namespace Shop.Application.Catalog.Categories.Commands.Handlers
{
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, int>
    {
        private readonly IMediator _mediator;
        private readonly ShopDbContext _context;
        private readonly ISlugService _slugService;
        private readonly IPictureService _pictureService;
        private readonly ICategoryService _categoryService;
        private readonly ITranslationEntityService _translationEntityService;

        public UpdateCategoryCommandHandler(
            IMediator mediator,
            ShopDbContext context,
            ISlugService slugService,
            IPictureService pictureService,
            ICategoryService categoryService,
            ITranslationEntityService translationEntityService)
        {
            _mediator = mediator;
            _context = context;
            _slugService = slugService;
            _pictureService = pictureService;
            _categoryService = categoryService;
            _translationEntityService = translationEntityService;
        }



        public async Task<int> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            Guard.IsNotNull(request, nameof(request));

            var category = await _categoryService.GetCategoryByIdAsync(request.Id, tracked: true);

            if (category == null)
                throw new NotFoundException();

            using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

            category.Name = request.Name;
            category.Description = request.Description;
            category.ShortDescription = request.ShortDescription;
            category.IsActive = request.IsActive;
            category.IsInlcudeTopMenu = request.IsInlcudeTopMenu;
            category.IsShowOnHomePage = request.IsShowOnHomePage;
            category.MetaTitle = request.MetaTitle;
            category.MetaKeywords = request.MetaKeywords;
            category.MetaDescription = request.MetaDescription;

           
            if (request.Slug.IsEmpty())
                request.Slug = await _slugService.ValidateSlugAsync(category, category.Name);
            else
                request.Slug = await _slugService.ValidateSlugAsync(category, request.Slug);

            
            int prevPictureId = category.PictureId ?? 0;
            if (prevPictureId != 0 && prevPictureId != request.PictureId)
            {
                var prevPicture = await _pictureService.GetPictureByIdAsync(prevPictureId);
                await _pictureService.DeleteAsync(prevPicture);
            }

            category.PictureId = request.PictureId == 0 ? null : request.PictureId;
            if(category.PictureId != null)
            {
                var picture = await _pictureService.GetPictureByIdAsync(category.PictureId.Value);
                await _pictureService.SetNamePictureAsync(picture, category.Name);
            }   

            category.ParentCategoryId = request.ParentCategoryId == 0 ? null : request.ParentCategoryId;
            
            var parentCategory = await _categoryService.GetCategoryByIdAsync(category.ParentCategoryId ?? 0);
            while (parentCategory != null)
            {
                if (category.Id == parentCategory.Id)
                {
                    category.ParentCategoryId = null;
                    break;
                }

                parentCategory = await _categoryService.GetCategoryByIdAsync(parentCategory.ParentCategoryId ?? 0);
            }

            await _translationEntityService.SaveTranslationPropertyAsync(category, p => p.Name, category.Name, 0);
            await _translationEntityService.SaveTranslationPropertyAsync(category, p => p.Description, category.Description, 0);
            await _translationEntityService.SaveTranslationPropertyAsync(category, p => p.ShortDescription, category.ShortDescription, 0);
            await _translationEntityService.SaveTranslationPropertyAsync(category, p => p.MetaTitle, category.MetaTitle, 0);
            await _translationEntityService.SaveTranslationPropertyAsync(category, p => p.MetaKeywords, category.MetaKeywords, 0);
            await _translationEntityService.SaveTranslationPropertyAsync(category, p => p.MetaDescription, category.MetaDescription, 0);

            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            await _mediator.EntityUpdated(category);

            return category.Id;
        }
    }
}
