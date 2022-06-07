using Shop.Domain.Catalog;

namespace Shop.Application.Catalog.Services
{
    public class CategoryService : AbstractService<Category>, ICategoryService
    {
        public CategoryService(ShopDbContext context) : base(context)
        {
        }
    }
}
