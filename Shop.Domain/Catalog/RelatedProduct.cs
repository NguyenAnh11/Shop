using Shop.SharedKernel;

namespace Shop.Domain.Catalog
{
    public class RelatedProduct : BaseEntity
    {
        public int Product1Id { get; set; }
        public Product Product1 { get; set; }
        public int Product2Id { get; set; }
        public Product Product2 { get; set; }
    }
}
