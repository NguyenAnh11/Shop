using Shop.SharedKernel;
using Shop.Domain.Localization;
using Shop.Domain.Media;

namespace Shop.Domain.Catalog
{
    public class SpecificationAttributeOption : BaseEntity, ITranslationEntity
    {
        public int SpecificationAttributeId { get; set; }
        public SpecificationAttribute SpecificationAttribute { get; set; } 
        public string Value { get; set; }
        public int DisplayOrder { get; set; }
        public string Color { get; set; }
        public int? PictureId { get; set; }
        public Picture Picture { get; set; }
    }
}
