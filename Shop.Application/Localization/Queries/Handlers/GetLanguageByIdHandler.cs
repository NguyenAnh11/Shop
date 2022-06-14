using Shop.Application.Localization.Queries.Models;
using Shop.Application.Localization.Dtos;
using Shop.Application.Localization.Services;

namespace Shop.Application.Localization.Queries.Handlers
{
    public class GetLanguageByIdHandler : IRequestHandler<GetLanguageByIdQuery, LanguageDto>
    {
        private readonly IMapper _mapper;
        private readonly ILanguageService _languageService;

        public GetLanguageByIdHandler(
            IMapper mapper,
            ILanguageService languageService)
        {
            _mapper = mapper;
            _languageService = languageService;
        }

        public async Task<LanguageDto> Handle(GetLanguageByIdQuery request, CancellationToken cancellationToken)
        {
            var language = await _languageService.GetLanguageByIdAsync(request.Id);

            if (language is null)
                throw new NotFoundException();

            var model = _mapper.Map<LanguageDto>(language);

            return model;
        }
    }
}
