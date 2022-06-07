using Shop.Domain.Localization;
using Shop.SharedKernel;
using Shop.SharedKernel.Interfaces;

namespace Shop.Domain.Seo
{
    public class Slug : BaseEntity, IActive
    {
        public int EntityId { get; set; }
        public string EntityGroup { get; set; }
        public string Value { get; set; }
        public int? LanguageId { get; set; }
        public Language Language { get; set; }
        public bool IsActive { get; set; }
    }
}
