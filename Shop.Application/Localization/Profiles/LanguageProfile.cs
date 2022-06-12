using Shop.Domain.Localization;
using Shop.Application.Localization.Dtos;

namespace Shop.Application.Localization.Profiles
{
    public class LanguageProfile : Profile, IProfile
    {
        public LanguageProfile()
        {
            CreateMap<Language, LanguageDto>();
        }
    }
}
