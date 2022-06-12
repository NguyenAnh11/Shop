namespace Shop.Application.Localization.Dtos
{
    public class TranslationResourceDto : BaseDto
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public int LanguageId { get; set; }
    }
}
