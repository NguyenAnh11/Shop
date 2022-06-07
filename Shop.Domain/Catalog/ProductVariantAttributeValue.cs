using Shop.SharedKernel;
using Shop.Domain.Media;

namespace Shop.Domain.Catalog
{
    public class ProductVariantAttributeValue : BaseEntity
    {
        public int ProductVariantAttributeId { get; set; }
        public ProductVariantAttrbiute ProductVariantAttrbiute { get; set; }    
        public string Value { get; set; }
        public int Quality { get; set; }
        public bool IsPreSelected { get; set; }
        public decimal PriceAdjustment { get; set; }
        public bool PriceAdjustmentUsePercentage { get; set; }
        public decimal WeightAdjustment { get; set; }
        public decimal HeightAdjustment { get; set; }
        public decimal WidthAdjustment { get; set; }
        public int DisplayOrder { get; set; }
        public int? PictureId { get; set; }
        public Picture Picture { get; set; }
        public int? ProductId { get; set; }
        public Product Product { get; set; }
        public int VariantTypeId { get; set; }
        public ProductVariantAttributeValueType VariantType
        {
            get => (ProductVariantAttributeValueType)VariantTypeId;
            set => VariantTypeId = (int)value;
        }
    }
}
