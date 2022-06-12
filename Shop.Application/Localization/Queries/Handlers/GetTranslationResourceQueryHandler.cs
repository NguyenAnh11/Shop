﻿using Shop.Application.Localization.Queries.Models;
using Shop.Application.Localization.Dtos;
using Shop.Application.Localization.Services;

namespace Shop.Application.Localization.Queries.Handlers
{
    public class GetTranslationResourceQueryHandler : IRequestHandler<GetTranslationResourceQuery, TranslationResourceDto>
    {
        private readonly IMapper _mapper;   
        private readonly ITranslationService _translationService;   

        public GetTranslationResourceQueryHandler(
            IMapper mapper, 
            ITranslationService translationService)
        {
            _mapper = mapper;
            _translationService = translationService;
        }

        public async Task<TranslationResourceDto> Handle(GetTranslationResourceQuery request, CancellationToken cancellationToken)
        {
            var translation = await _translationService.GetTranslationByIdAsync(request.Id);

            if (translation == null)
                throw new NotFoundException();

            var model = _mapper.Map<TranslationResourceDto>(translation);

            return model;
        }
    }
}
