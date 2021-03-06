using Shop.Domain.Directory;
using Shop.SharedKernel;
using Shop.SharedKernel.Interfaces;

namespace Shop.Domain.Localization
{
    public class Language : BaseEntity, IActive
    {
        public string Name { get; set; }
        public bool IsRtl { get; set; }
        public bool IsActive { get; set; }
        public string Code { get; set; }
        public string Culture { get; set; }
        public int DisplayOrder { get; set; }
        public int? CurrencyId { get; set; }
        public Currency Currency { get; set; }
    }
}
