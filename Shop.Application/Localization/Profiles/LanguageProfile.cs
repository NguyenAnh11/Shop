using Shop.Application.Localization.Dtos;
using Shop.Domain.Localization;

namespace Shop.Application.Localization.Profiles
{
    public class LanguageProfile : Profile, IProfile
    {
        public LanguageProfile()
        {
            CreateMap<Language, LanguageDto>()
                .ForMember(p => p.CurrencyId, s => s.MapFrom(s => s.CurrencyId ?? 0));
        }
    }
}
