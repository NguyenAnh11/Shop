using Shop.Domain.Catalog;

namespace Shop.Application.Catalog.Categories.Services
{
    public interface ICategoryService : IAbstractService<Category>
    {
        Task<Category> GetCategoryByIdAsync(int id, bool includeDeleted = false, bool tracked = false);

        Task<IList<Category>> GetCategoriesByParentCategoryIdAsync(int? parentCategoryId = null, bool includeHidden = false, bool tracked = false);

        Task<IList<Category>> GetCategoriesDisplayOnHomePageAsync(bool includeHidden = false);

        Task<IList<Category>> GetCategoryBreadCrumbAsync(Category category, IList<Category> categories = null, bool includeHidden = true);

        Task<string> GetFormatBreadCrumbAsync(Category category, IList<Category> categories = null, int? languageId = null, string separator = "->");

        Task<IList<int>> GetCategoryIdsByProductAsync(Product product, bool includeHidden = false);
    }
}
