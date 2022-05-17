using Shop.Application.Localization.Dtos;
using Shop.Domain.Localization;

namespace Shop.Application.Localization.Services
{
    public class LocalizationService : AbstractService<LocaleStringResource>, ILocalizationService
    {
        public LocalizationService(ShopDbContext context) : base(context)
        {
        }

        public async Task<LocaleStringResource> GetResourceByNameAsync(string name, int languageId, bool extractMatch = true)
        {
            if (name.IsEmpty() || languageId <= 0)
                return null;

            var query = Table
                .AsNoTracking()
                .Where(p => p.LanguageId == languageId);

            if (extractMatch)
                query = query.ApplyPatternFilter(p => p.Name == name);
            else
                query = query.ApplyPatternFilter(p => EF.Functions.Like(p.Name, $"%{name}%"));

            var lsr = await query.FirstOrDefaultAsync();

            return lsr;
        }

        public async Task<string> GetResourceAsync(string name)
            => await GetResourceAsync(name, 1);

        public async Task<string> GetResourceAsync(string name, int langaugeId)
            => (await GetResourceByNameAsync(name, langaugeId))?.Value ?? name;

        public async Task<string> GetLocalizedEnumAsync<TEnum>(TEnum enumValue)
            where TEnum : struct, ILocalizedEnum
            => await GetLocalizedEnumAsync(enumValue, 1);

        public async Task<string> GetLocalizedEnumAsync<TEnum>(TEnum enumValue, int languageId)
            where TEnum: struct, ILocalizedEnum
        {
            if (!typeof(TEnum).IsEnum)
                throw new InvalidTypeException();

            var name = string.Join('.', typeof(TEnum), enumValue);

            if (languageId <= 0)
                return name;

            return (await GetResourceByNameAsync(name, languageId))?.Value ?? name;
        }

        public async Task<LocaleResourceDto> GetResourceByIdAsync(int id)
        {
            var resource = await Table.FindByIdAsync(id);

            if (resource == null)
                throw new NotFoundException();

            return new LocaleResourceDto
            {
                Id = resource.Id,
                Name = resource.Name,
                Value = resource.Value,
                LanguageId = resource.LanguageId
            };
        }

        public async Task<Response<int>> InsertResourceAsync(LocaleResourceDto dto)
        {
            Guard.IsNotNull(dto, nameof(dto));

            var language = await _context.Set<Language>().FindByIdAsync(dto.LanguageId);

            if (language == null)
                return Response<int>.Bad();

            var resource = await GetResourceByNameAsync(dto.Name, dto.LanguageId);

            if (resource != null)
            {
                return Response<int>.Bad("Resource.Error.NameAlreadyExist");
            }

            using var transaction = await _context.Database.BeginTransactionAsync();

            resource = new LocaleStringResource
            {
                Value = dto.Value,
                Name = dto.Name.ToLower(),
                LanguageId = dto.LanguageId
            };

            await Table.AddAsync(resource);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return Response<int>.Ok(resource.Id);
        }

        public async Task<Response> UpdateResourceAsync(LocaleResourceDto dto)
        {
            Guard.IsNotNull(dto, nameof(dto));

            var resource = await Table.FindByIdAsync(dto.Id, tracked: true);

            if (resource == null)
                throw new NotFoundException();

            var language = await _context.Set<Language>().FindByIdAsync(dto.LanguageId);

            if (language == null)
                return Response.Bad();

            if (!resource.Name.EqualsNoCase(dto.Name))
            {
                if (await GetResourceByNameAsync(dto.Name, dto.LanguageId) != null)
                {
                    return Response.Bad("Resource.Error.NameAlreadyExist");
                }
            }

            using var transaction = await _context.Database.BeginTransactionAsync();

            resource.Value = dto.Value;
            resource.Name = dto.Name.ToLower();
            resource.LanguageId = dto.LanguageId;

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return Response.Ok();
        }

        public async Task<Response> DeleteResourceAsync(int id)
        {
            var resource = await Table.FindByIdAsync(id);

            if (resource == null)
                throw new NotFoundException();

            using var transaction = await _context.Database.BeginTransactionAsync();

            Table.Remove(resource);

            await transaction.CommitAsync();

            return Response.Ok();
        }
    }
}
