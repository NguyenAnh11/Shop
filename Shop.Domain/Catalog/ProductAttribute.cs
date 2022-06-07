using Shop.SharedKernel;
using Shop.Domain.Localization;

namespace Shop.Domain.Catalog
{
    public class ProductAttribute : BaseEntity, ILocalizedEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
