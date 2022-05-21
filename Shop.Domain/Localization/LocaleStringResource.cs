using Shop.SharedKernel;

namespace Shop.Domain.Localization
{
    public class LocaleStringResource : BaseEntity
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public int LanguageId { get; set; }
        public Language Language { get; set; }
    }
}
