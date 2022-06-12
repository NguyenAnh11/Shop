using Shop.Application.Localization.Queries.Models;
using Shop.Application.Localization.Dtos;
using Shop.Application.Localization.Services;

namespace Shop.Application.Localization.Queries.Handlers
{
    public class GetLanguageQueryHandler : IRequestHandler<GetLanguageQuery, LanguageDto>
    {
        private readonly IMapper _mapper;
        private readonly ILanguageService _languageService;

        public GetLanguageQueryHandler(
            IMapper mapper,
            ILanguageService languageService)
        {
            _mapper = mapper;
            _languageService = languageService;
        }

        public async Task<LanguageDto> Handle(GetLanguageQuery request, CancellationToken cancellationToken)
        {
            var language = await _languageService.GetLanguageByIdAsync(request.Id);

            if (language is null)
                throw new NotFoundException();

            var model = _mapper.Map<LanguageDto>(language);

            return model;
        }
    }
}
