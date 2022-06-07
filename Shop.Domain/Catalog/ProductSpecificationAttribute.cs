using Shop.SharedKernel;

namespace Shop.Domain.Catalog
{
    public class ProductSpecificationAttribute : BaseEntity
    {
        public int ProductId { get; set; }  
        public Product Product { get; set; }
        public int SpecificationAttributeOptionId { get; set; } 
        public SpecificationAttributeOption SpecificationAttributeOption { get; set; }
        public bool IsShowOnProductPage { get; set; }
        public bool IsAllowFiltering { get; set; }
        public int DisplayOrder { get; set; }
    }
}
