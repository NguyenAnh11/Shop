namespace Shop.Application.Localization.Dtos
{
    public class LanguageDto : BaseDto
    {
        public string Name { get; set; }
        public bool IsRtl { get; set; }
        public bool IsActive { get; set; }
        public string Code { get; set; }
        public string Culture { get; set; }
        public int CurrencyId { get; set; }
        public int DisplayOrder { get; set; }
    }
}
