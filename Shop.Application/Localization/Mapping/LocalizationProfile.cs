namespace Shop.Application.Localization.Mapping
{
    public class LocalizationProfile : Profile, IProfile
    {
        public LocalizationProfile()
        {
            CreateMap<Language, LanguageDto>()
                .ForMember(d => d.CurrencyId, s => s.PreCondition(s => s.CurrencyId.HasValue));

            CreateMap<LanguageDto, Language>()
                .ForMember(d => d.Code, s => s.MapFrom(s => s.Code.ToLower()))
                .ForMember(d => d.Culture, s => s.MapFrom(s => s.Culture.ToLower()))
                .ForMember(d => d.CurrencyId, s => s.PreCondition(s => s.CurrencyId != 0));
        }
    }
}
