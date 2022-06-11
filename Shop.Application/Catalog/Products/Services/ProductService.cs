using Shop.Application.Media.Services;
using Shop.Domain.Catalog;
using Shop.Domain.Media;

namespace Shop.Application.Catalog.Products.Services
{
    public class ProductService : AbstractService<Product>, IProductService
    {
        private readonly IPictureService _pictureService;
        public ProductService(ShopDbContext context, IPictureService pictureService) : base(context)
        {
            _pictureService = pictureService;
        }

        public async Task<Product> GetProductByIdAsync(int id, bool includeDeleted = false, bool tracked = false)
            => await Table.FindByIdAsync(id, includeDeleted, tracked);

        public async Task<IList<Product>> GetProductsDisplayOnHomePage(bool includeHidden = false)
        {
            var products = await Table
                .ApplyTracking()
                .ApplyDeletedFilter()
                .ApplyActiveFilter(includeHidden)
                .Where(p => p.IsShowOnHomePage)
                .ToListAsync();

            return products;
        }

        public async Task<Product> GetProductBySkuAsync(string sku, bool includeHidden = false)
        {
            if (sku.IsEmpty())
                return null;

            var product = await Table
                .ApplyTracking()
                .ApplyDeleteFilter()
                .ApplyActiveFilter(includeHidden)
                .ApplyPatternFilter(p => p.SKu == sku)
                .FirstOrDefaultAsync();

            return product;
        }

        public async Task<IList<Picture>> GetProductPictureAsync(Product product, int recordToReturn = 0)
        {
            Guard.IsNotNull(product, nameof(product));
            Guard.IsGreaterThanOrEqualTo(recordToReturn, 0, nameof(recordToReturn));

            var ids = await _context.Set<ProductPicture>()
                .Where(p => p.ProductId == product.Id)
                .Select(p => p.PictureId)
                .ToListAsync();

            var query = _pictureService.Table.Where(p => ids.Contains(p.Id));

            if (recordToReturn > 0)
                query = query.Take(recordToReturn);

            return await query.ToListAsync();
        }
    }
}
