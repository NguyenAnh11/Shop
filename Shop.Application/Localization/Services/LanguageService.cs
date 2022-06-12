using Shop.Application.Configurations.Services;
using Shop.Application.Localization.Settings;
using Shop.Domain.Localization;

namespace Shop.Application.Localization.Services
{
    public class LanguageService : AbstractService<Language>, ILanguageService
    {
        public LanguageService(ShopDbContext context) : base(context)
        {
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

    }
}
