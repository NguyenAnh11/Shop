namespace Shop.Application.Localization.Services
{
    public class LanguageService : ILanguageService
    { 
        private readonly IRepository<Language> _languageRepository;
        public LanguageService(IRepository<Language> languageRepository)
        {
            _languageRepository = languageRepository;
        }

        public async Task<Language> GetLanguageByIdAsync(int id)
        {
            var language = await _languageRepository.GetByIdAsync(id);

            return language;
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
            
            language = dto.ToModel(language);

            await _languageRepository.InsertAsync(language);

            await transaction.CommitAsync();

            return Response<Language>.Ok(language);
        }

        public async Task<Response> UpdateLanguageAsync(int id, LanguageDto dto)
        {
            Guard.IsNotNull(dto, nameof(dto));
            
            if(id != dto.Id)
            {
                return Response.Bad();
            }

            var language = await GetLanguageByIdAsync(id);

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

            language = dto.MapTo(language);

            await _languageRepository.UpdateAsync(language);

            await transaction.CommitAsync();

            return Response.Ok();
        }

        public async Task<Response> DeleteLanguageAsync(int id)
        {
            var language = await GetLanguageByIdAsync(id);

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
