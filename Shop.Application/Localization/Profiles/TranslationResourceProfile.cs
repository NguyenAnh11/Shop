using Shop.Application.Localization.Dtos;
using Shop.Domain.Localization;

namespace Shop.Application.Localization.Profiles
{
    public class TranslationResourceProfile : Profile, IProfile
    {
        public TranslationResourceProfile()
        {
            CreateMap<TranslationResource, TranslationResourceDto>();
        }
    }
}
