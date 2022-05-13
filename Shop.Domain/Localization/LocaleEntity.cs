using Shop.SharedKernel;

namespace Shop.Domain.Localization
{
    public class LocaleEntity : BaseEntity
    {
        public int? LanguageId { get; set; }
        public Language Language { get; set; }
        public int EntityId { get; set; }
        public string EntityGroup { get; set; }
        public string EntityKey { get; set; }
        public string EntityValue { get; set; }
    }
}
