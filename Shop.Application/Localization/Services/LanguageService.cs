using Shop.Domain.Localization;

namespace Shop.Application.Localization.Services
{
    public class LanguageService : ILanguageService
    { 
        private readonly IRepository<Language> _languageRepository;
        public LanguageService(IRepository<Language> languageRepository)
        {
            _languageRepository = languageRepository;
        }

        public async Task<LanguageDto> GetLanguageByIdAsync(int id)
        {
            var language = await _languageRepository.GetByIdAsync(id);

            var model = new LanguageDto
            {
                Id = language.Id,
                Name = language.Name,
                IsRtl = language.IsRtl,
                IsActive = language.IsActive,
                Code = language.Code,
                Culture = language.Culture,
                CurrencyId = language.CurrencyId ?? 0,
                Flag = language.Flag,
                DisplayOrder = language.DisplayOrder
            };

            return model;
        }

        public async Task<Language> GetLanguageByCodeAsync(string code)
        {
            if (code.IsEmpty())
                return null;

            code = code.ToLower();

            var language = await _languageRepository.Table.FirstOrDefaultAsync(p => p.Code == code);

            return language;
        }

        public async Task<IList<Language>> GetActiveLanguageAsync()
        {
            var languages = await _languageRepository.Table.Where(p => p.IsActive).ToListAsync();

            return languages;
        }

        public async Task<Response<Language>> InsertLanguageAsync(LanguageDto dto)
        {
            Guard.IsNotNull(dto, nameof(dto));

            var language = await GetLanguageByCodeAsync(dto.Code);

            if(language != null)
            {
                return Response<Language>.Bad("Language.Error.CodeAlreadyExist");
            }

            using var transaction = await _languageRepository.BeginTransactionAsync();

            language = new Language
            {
                Name = dto.Name,
                IsRtl = dto.IsRtl,
                IsActive = dto.IsActive,
                Code = dto.Code.ToLower(),
                Culture = dto.Culture,
                Flag = dto.Flag,
                DisplayOrder = dto.DisplayOrder
            };

            if (dto.CurrencyId == 0)
                language.CurrencyId = null;

            await _languageRepository.InsertAsync(language);

            await transaction.CommitAsync();

            return Response<Language>.Ok(language);
        }

        public async Task<Response> UpdateLanguageAsync(int id, LanguageDto dto)
        {
            Guard.IsNotNull(dto, nameof(dto));
            
            var language = await _languageRepository.GetByIdAsync(id);

            Guard.IsNotNull(language, nameof(language));

            if(!language.Code.EqualsNoCase(dto.Code))
            {
                return Response.Bad("Language.Error.CodeAlreadyExist");
            }

            if (language.IsActive && !dto.IsActive)
            { 
                var activeLanguages = await GetActiveLanguageAsync();

                if (activeLanguages.Count == 1 && activeLanguages[0].Id == language.Id)
                {
                    return Response.Bad("Language.Error.AtLeastActive");
                }
            }

            using var transaction = await _languageRepository.BeginTransactionAsync();

            language.Name = dto.Name;
            language.IsRtl = dto.IsRtl;
            language.IsActive = dto.IsActive;
            language.Code = dto.Code.ToLower();
            language.Culture = dto.Culture;
            language.Flag = dto.Flag;
            language.DisplayOrder = dto.DisplayOrder;

            if (dto.CurrencyId != 0)
                language.CurrencyId = null;

            await _languageRepository.Update(language);

            await transaction.CommitAsync();

            return Response.Ok();
        }

        public async Task<Response> DeleteLanguageAsync(int id)
        {
            var language = await _languageRepository.GetByIdAsync(id);

            Guard.IsNotNull(language, nameof(language));

            if(language.IsActive)
            {
                var activeLanguages = await GetActiveLanguageAsync();

                if (activeLanguages.Count == 1 && activeLanguages[0].Id == language.Id)
                {
                    return Response.Bad("Language.Error.AtLeastActive");
                }
            }

            using var transaction = await _languageRepository.BeginTransactionAsync();

            await _languageRepository.DeleteAsync(language);

            await transaction.CommitAsync();

            return Response.Ok();
        }
    }
}
