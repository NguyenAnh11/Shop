using Shop.SharedKernel;
using Shop.Domain.Directory;

namespace Shop.Domain.Localization
{
    public class Language : BaseEntity
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public bool IsRtl { get; set; }
        public int? CurrencyId { get; set; }
        public Currency Currency { get; set; }
        public string Code { get; set; }
        public string Culture { get; set; }
        public string Flag { get; set; }
        public int DisplayOrder { get; set; }
    }
}
