using Shop.SharedKernel;

namespace Shop.Domain.Catalog
{
    public class ProductVariantAttrbiute : BaseEntity
    {
        public int ProductAttributeId { get; set; }
        public ProductAttribute ProductAttribute { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public bool IsRequired { get; set; }
        public int DisplayOrder { get; set; }
        public string TextPrompt { get; set; }
        public int ControlTypeId { get; set; }
        public AttributeControlType ControlType
        {
            get => (AttributeControlType)ControlTypeId;
            set => ControlTypeId = (int)value;
        }

        public IList<ProductVariantAttributeValue> ProductVariantAttributeValues { get; set; } = new List<ProductVariantAttributeValue>();
    }
}
