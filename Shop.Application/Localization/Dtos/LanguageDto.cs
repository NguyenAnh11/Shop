namespace Shop.Application.Localization.Dtos
{
    public record LanguageDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public bool IsRtl { get; set; }
        public int CurrencyId { get; set; }
        public string Code { get; set; }
        public string Culture { get; set; }
        public string Flag { get; set; }
        public int DisplayOrder { get; set; }
    }
}
