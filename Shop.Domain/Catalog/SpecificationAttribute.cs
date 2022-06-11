using Shop.SharedKernel;
using Shop.Domain.Localization;

namespace Shop.Domain.Catalog
{
    public class SpecificationAttribute : BaseEntity, ITranslationEntity
    {
        public string Name { get; set; }
        public string Description { get; set; } 
        public int DisplayOrder { get; set; }
        public bool IsShowOnHomePage { get; set; }
        public bool IsAllowFilter { get; set; }
        public int SpecificationAttributeGroupId { get; set; }
        public SpecificationAttributeGroup SpecificationAttributeGroup { get; set; }
        public IList<SpecificationAttributeOption> SpecificationAttributeOptions { get; set; } = new List<SpecificationAttributeOption>();
    }
}
