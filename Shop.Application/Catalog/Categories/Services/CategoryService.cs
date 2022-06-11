using Shop.Application.Localization.Services;
using Shop.Domain.Catalog;

namespace Shop.Application.Catalog.Categories.Services
{
    public class CategoryService : AbstractService<Category>, ICategoryService
    {
        private readonly ITranslationEntityService _translationEntityService;
        public CategoryService(
            ShopDbContext context,
            ITranslationEntityService translationEntityService) : base(context)
        {
            _translationEntityService = translationEntityService;
        }

        public async Task<Category> GetCategoryByIdAsync(int id, bool includeDeleted = false, bool tracked = false)
            => await Table.FindByIdAsync(id, includeDeleted, tracked);

        public async Task<IList<Category>> GetCategoriesByParentCategoryIdAsync(int? parentCategoryId = null, bool includeHidden = false, bool tracked = false)
        {
            var categories = await Table
                .ApplyTracking(tracked)
                .ApplyDeletedFilter()
                .ApplyActiveFilter(includeHidden)
                .Where(p => p.ParentCategoryId == parentCategoryId)
                .OrderBy(p => p.DisplayOrder)
                .ToListAsync();

            return categories;
        }

        public async Task<IList<Category>> GetCategoriesDisplayOnHomePageAsync(bool includeHidden = false)
        {
            var categories = await Table
                .ApplyTracking()
                .ApplyDeletedFilter()
                .ApplyActiveFilter(includeHidden)
                .Where(p => p.IsShowOnHomePage)
                .OrderBy(p => p.DisplayOrder)
                .ToListAsync();

            return categories;
        }

        public async Task<IList<Category>> GetCategoryBreadCrumbAsync(Category category, IList<Category> categories = null, bool includeHidden = false)
        {
            Guard.IsNotNull(category, nameof(category));

            var breadcrumb = new List<Category>();

            while (category != null && !category.IsDelete)
            {
                if (!includeHidden && !category.IsActive)
                    break;

                breadcrumb.Add(category);

                int? parentId = category.ParentCategoryId;
                if (parentId == null)
                    break;

                category = categories != null ?
                    categories.FirstOrDefault(p => p.ParentCategoryId == parentId.Value) :
                    await GetCategoryByIdAsync(parentId.Value);
            }

            breadcrumb.Reverse();

            return breadcrumb;
        }

        public async Task<string> GetFormatBreadCrumbAsync(Category category, IList<Category> categories = null, int? languageId = null, string separator = "->")
        {
            Guard.IsNotNull(category, nameof(category));

            string path = string.Empty;

            var breadcrumb = await GetCategoryBreadCrumbAsync(category, categories);

            for (int i = 0; i < breadcrumb.Count; i++)
            {
                var categoryName = await _translationEntityService.GetTranslationPropertyAsync(breadcrumb[i], p => p.Name, languageId);
                path += i == 0 ? categoryName : separator + categoryName;
            }

            return path;
        }

        public async Task<IList<int>> GetCategoryIdsByProductAsync(Product product, bool includeHidden = false)
        {
            Guard.IsNotNull(product, nameof(product));

            var query = _context.Set<ProductCategory>()
                .Where(p => p.ProductId == product.Id)
                .Join(
                    Table
                        .ApplyDeletedFilter()
                        .ApplyActiveFilter(includeHidden),
                    pc => pc.CategoryId,
                    c => c.Id,
                    (pc, c) => c.Id
                );

            var ids = await query.ToListAsync();
            return ids;
        }
    }
}
