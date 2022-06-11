using Shop.Domain.Catalog;
using Shop.Domain.Media;

namespace Shop.Application.Catalog.Products.Services
{
    public interface IProductService : IAbstractService<Product>
    {
        Task<Product> GetProductByIdAsync(int id, bool includDeleted = false, bool tracked = false);

        Task<IList<Product>> GetProductsDisplayOnHomePage(bool includeHidden = false);

        Task<Product> GetProductBySkuAsync(string sku, bool includeHidden = false);

        Task<IList<Picture>> GetProductPictureAsync(Product product, int recordToReturn = 0);
    }
}
