using Shop.Application.Localization.Dtos;
using Shop.Domain.Localization;
using Shop.Application.Configurations.Services;
using Shop.Application.Localization.Settings;

namespace Shop.Application.Localization.Services
{
    public class LanguageService : AbstractService<Language>, ILanguageService
    {
        private readonly ISettingService _settingService;
        private readonly LocalizationSetting _localizationSetting;
        public LanguageService(
            ShopDbContext context, 
            ISettingService settingService,
            LocalizationSetting localizationSetting) : base(context)
        {
            _settingService = settingService;
            _localizationSetting = localizationSetting;
        }

        public async Task<Language> GetLanguageByCodeAsync(string code)
        {
            if (code.IsEmpty())
                return null;

            var query = Table.AsNoTracking().ApplyPatternFilter(p => EF.Functions.Like(p.Code, $"%{code}"));

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Language> GetLanguageByIdAsync(int id, bool tracked = false)
            => await Table.FindByIdAsync(id, tracked: tracked);

        public async Task<IList<Language>> GetLanguagesAsync(bool includeHidden = true, bool isRtl = false, bool tracked = false)
        {
            var languages = await Table
                .ApplyTracking(tracked)
                .Where(p => p.IsRtl == isRtl)
                .ApplyActiveFilter(includeHidden)
                .OrderBy(p => p.DisplayOrder)
                .ToListAsync();

            return languages;
        }

        public async Task<Response<int>> InsertLanguageAsync(LanguageDto dto)
        {
            Guard.IsNotNull(dto, nameof(dto));

            var language = await GetLanguageByCodeAsync(dto.Code);

            if (language != null)
            {
                return Response<int>.Bad("Language.Error.AlreadyCodeExist");
            }

            using var transaction = await _context.Database.BeginTransactionAsync();

            language = new Language
            {
                Name = dto.Name,
                IsRtl = dto.IsRtl,
                IsActive = dto.IsActive,
                Code = dto.Code.ToLower(),
                Culture = dto.Culture,
                DisplayOrder = dto.DisplayOrder
            };

            if (dto.CurrencyId == 0)
                language.CurrencyId = null;

            await Table.AddAsync(language);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return Response<int>.Ok(language.Id);
        }

        public async Task<Response> UpdateLanguageAsync(LanguageDto dto)
        {
            Guard.IsNotNull(dto, nameof(dto));

            var language = await Table.FindByIdAsync(dto.Id, tracked: true);

            if (language == null)
                throw new NotFoundException();

            if (!language.Code.EqualsNoCase(dto.Code))
            {
                if (await GetLanguageByCodeAsync(dto.Code) != null)
                {
                    return Response.Bad("Language.Error.AlreadyCodeExist");
                }
            }

            if (language.IsActive && !dto.IsActive)
            {
                var languages = await GetLanguagesAsync(false);

                if (languages.Count == 1 && languages[0].Id == language.Id)
                {
                    return Response.Bad("Language.Error.RequireAtLeastActive");
                }
            }

            using var transaction = await _context.Database.BeginTransactionAsync();

            language.Name = dto.Name;
            language.IsRtl = dto.IsRtl;
            language.IsActive = dto.IsActive;
            language.Code = dto.Code.ToLower();
            language.Culture = dto.Culture;
            language.DisplayOrder = dto.DisplayOrder;

            if (dto.CurrencyId == 0)
                language.CurrencyId = null;

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return Response.Ok();
        }

        public async Task<Response> DeleteLanguageAsync(Language language)
        {
            Guard.IsNotNull(language, nameof(language));

            using var transaction = await _context.Database.BeginTransactionAsync();

            if (language.IsActive)
            {
                var languages = await GetLanguagesAsync(false, tracked: true);

                if(languages.Count == 1 && languages[0].Id == language.Id)
                {
                    return Response.Bad("Language.Error.RequireAtLeastActive");
                }

                if(_localizationSetting.DefaultLanguageAdminId == language.Id)
                {
                    _localizationSetting.DefaultLanguageAdminId = language.Id;
                    await _settingService.SaveSettingAsync(_localizationSetting, p => p.DefaultLanguageAdminId);
                }
            }

            Table.Remove(language);

            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

            return Response.Ok();
        }
    }
}
