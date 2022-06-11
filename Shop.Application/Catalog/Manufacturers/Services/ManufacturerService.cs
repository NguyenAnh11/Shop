using Shop.Domain.Catalog;

namespace Shop.Application.Catalog.Manufacturers.Services
{
    public class ManufacturerService : AbstractService<Manufacturer>, IManufacturerService
    {
        public ManufacturerService(ShopDbContext context) : base(context)
        {
        }
    }
}
