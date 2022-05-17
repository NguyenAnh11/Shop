namespace Shop.Application.Localization.Dtos
{
    public record LocaleResourceDto
    {
        public int Id { get; set; }
        public int LanguageId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
