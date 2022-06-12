using Shop.Domain.Localization;

namespace Shop.Application.Localization.Services
{
    public class TranslationService : AbstractService<TranslationResource>, ITranslationService
    {
        private readonly IWorkContext _workContext;

        public TranslationService(ShopDbContext context, IWorkContext workContext) : base(context)
        {
            _workContext = workContext;
        }

        public async Task<TranslationResource> GetTranslationByIdAsync(int id, bool tracked = false)
            => await Table.FindByIdAsync(id, tracked: tracked);

        public async Task<TranslationResource> GetTranslationByNameAsync(string name, int languageId, bool extractMatch = true)
        {
            if (name.IsEmpty() || languageId <= 0)
                return null;

            var query = Table
                .ApplyTracking()
                .Where(p => p.LanguageId == languageId);

            if (extractMatch)
                query = query.ApplyPatternFilter(p => p.Name == name);
            else
                query = query.ApplyPatternFilter(p => EF.Functions.Like(p.Name, $"%{name}%"));

            var lsr = await query.FirstOrDefaultAsync();

            return lsr;
        }

        public async Task<int> GetCountTranslationsByLanguageIdAsync(int languageId)
        {
            Guard.IsGreaterThan(languageId, 0, nameof(languageId));

            var counts = await Table.CountAsync(p => p.LanguageId == languageId);

            return counts;
        }

        public async Task<string> GetTranslationAsync(string name)
            => await GetTranslationAsync(name, (await _workContext.GetWorkingLanguageAsync()).Id);

        public async Task<string> GetTranslationAsync(string name, int langaugeId)
            => (await GetTranslationByNameAsync(name, langaugeId))?.Value ?? name;

        public async Task<string> GetTranslationEnumAsync<TEnum>(TEnum enumValue)
            where TEnum : struct
            => await GetTranslationEnumAsync(enumValue, (await _workContext.GetWorkingLanguageAsync()).Id);

        public async Task<string> GetTranslationEnumAsync<TEnum>(TEnum enumValue, int languageId)
            where TEnum : struct
        {
            Guard.IsAssignableToType(typeof(TEnum), typeof(Enum), nameof(enumValue));

            var enumName = typeof(TEnum).Name.Split('.')?.Last();

            var name = string.Join('.', enumName, enumValue);

            if (languageId <= 0)
                return name;

            return (await GetTranslationByNameAsync(name, languageId))?.Value ?? name;
        }

    }
}
