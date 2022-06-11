using Shop.SharedKernel;
using Shop.Domain.Localization;

namespace Shop.Domain.Catalog
{
    public class SpecificationAttributeGroup : BaseEntity, ITranslationEntity
    {
        public string Name { get; set; }
        public int DisplayOrder { get; set; }
        public IList<SpecificationAttribute> SpecificationAttributes { get; set; } = new List<SpecificationAttribute>();
    }
}
