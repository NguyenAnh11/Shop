﻿using Shop.Application.Catalog.Categories.Commands.Models;
using Shop.Application.Localization.Services;
using Shop.Application.Media.Services;
using Shop.Application.Seo.Services;
using Shop.Domain.Catalog;

namespace Shop.Application.Catalog.Categories.Commands.Handlers
{
    public class AddCategoryCommandHandler : IRequestHandler<AddCategoryCommand, int>
    {
        private readonly IMediator _meidator;
        private readonly ShopDbContext _context;
        private readonly ISlugService _slugService;
        private readonly IPictureService _pictureService;
        private readonly ITranslationEntityService _translationEntityService;

        public AddCategoryCommandHandler(
            IMediator meidator,
            ShopDbContext context,
            ISlugService slugService,
            IPictureService pictureService,
            ITranslationEntityService translationEntityService)
        {
            _meidator = meidator;
            _context = context;
            _slugService = slugService;
            _pictureService = pictureService;
            _translationEntityService = translationEntityService;
        }

        public async Task<int> Handle(AddCategoryCommand request, CancellationToken cancellationToken)
        {
            Guard.IsNotNull(request, nameof(request));

            using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

            var category = new Category
            {
                Name = request.Name,
                Description = request.Description,
                ShortDescription = request.Description,
                IsActive = request.IsActive,
                IsInlcudeTopMenu = request.IsInlcudeTopMenu,
                IsShowOnHomePage = request.IsShowOnHomePage,
                MetaTitle = request.MetaTitle,
                MetaKeywords = request.MetaKeywords,
                MetaDescription = request.MetaDescription,
                PictureId = request.PictureId == 0 ? null : request.PictureId,
                ParentCategoryId = request.ParentCategoryId == 0 ? null : request.ParentCategoryId,
            };

            await _context.Set<Category>().AddAsync(category, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            if (request.Slug.IsEmpty())
                request.Slug = await _slugService.ValidateSlugAsync(category, category.Name);
            else
                request.Slug = await _slugService.ValidateSlugAsync(category, request.Slug);

            await _slugService.SaveSlugAsync(category, request.Slug, 0);

            if (category.PictureId != null)
            {
                var picture = await _pictureService.GetPictureByIdAsync(category.PictureId.Value, true);
                await _pictureService.SetNamePictureAsync(picture, request.Slug);
            }

            //save original translation
            await _translationEntityService.SaveTranslationPropertyAsync(category, p => p.Name, category.Name, 0);
            await _translationEntityService.SaveTranslationPropertyAsync(category, p => p.Description, category.Description, 0);
            await _translationEntityService.SaveTranslationPropertyAsync(category, p => p.ShortDescription, category.ShortDescription, 0);
            await _translationEntityService.SaveTranslationPropertyAsync(category, p => p.MetaTitle, category.MetaTitle, 0);
            await _translationEntityService.SaveTranslationPropertyAsync(category, p => p.MetaKeywords, category.MetaKeywords, 0);
            await _translationEntityService.SaveTranslationPropertyAsync(category, p => p.MetaDescription, category.MetaDescription, 0);

            await transaction.CommitAsync(cancellationToken);

            await _meidator.EntityUpdated(category);
            return category.Id;
        }
    }
}
