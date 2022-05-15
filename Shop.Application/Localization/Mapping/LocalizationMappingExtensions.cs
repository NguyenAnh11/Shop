namespace Shop.Application.Localization.Mapping
{
    public static class LocalizationMappingExtensions
    {
        public static LanguageDto ToModel(this Language language)
        {
            return language.MapTo<Language, LanguageDto>();
        }

        public static Language ToModel(this LanguageDto dto)
        {
            return dto.MapTo<LanguageDto, Language>();
        }

        public static Language ToModel(this LanguageDto dto, Language language)
        {
            return dto.ToModel(language);
        }
    }
}
