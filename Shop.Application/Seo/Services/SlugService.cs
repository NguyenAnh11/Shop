using Shop.Application.Seo.Helpers;
using Shop.Domain.Seo;

namespace Shop.Application.Seo.Services
{
    public class SlugService : AbstractService<Slug>, ISlugService
    {
        private readonly IWorkContext _workContext;
        public SlugService(ShopDbContext context, IWorkContext workContext) : base(context)
        {
            _workContext = workContext;
        }

        public async Task<Slug> GetSlugByIdAsync(int id, bool tracked = false)
            => await Table.FindByIdAsync(id, tracked: tracked);

        public async Task<Slug> GetBySlugAsync(string value)
        {
            if (value.IsEmpty())
                return null;

            var slug = await Table.FirstOrDefaultAsync(p => p.Value == value);

            return slug;
        }

         public async Task<IList<Slug>> GetSlugsAsync<T>(T entity, bool includeHidden = false, bool tracked = false) 
            where T: BaseEntity, ISlugSupported
        {
            Guard.IsNotNull(entity, nameof(entity));

            var slugs = await Table
                .ApplyTracking(tracked)
                .ApplyActiveFilter(includeHidden)
                .Where(p => p.EntityId == entity.Id && p.EntityGroup == typeof(T).Name)
                .ToListAsync();

            return slugs;
        }

        public async Task<IList<Slug>> GetSlugsAsync<T>(T entity, int? languageId, bool includeHidden = false, bool tracked = false)
            where T : BaseEntity, ISlugSupported
        {
            Guard.IsNotNull(entity, nameof(entity));
            Guard.IsGreaterThan(languageId.Value, 0, nameof(languageId));

            var entityId = entity.Id;
            var entityGroup = entity.GetType().Name;

            var slugs = await Table
                .ApplyTracking(tracked)
                .ApplyActiveFilter(includeHidden)
                .Where(p => p.EntityId == entityId && p.EntityGroup == entityGroup && p.LanguageId == languageId)
                .ToListAsync();

            return slugs;
        }

        public async Task<Slug> GetActiveSlugAsync(int entityId, string entityGroup, int? languageId)
        {
            Guard.IsGreaterThan(entityId, 0, nameof(entityId));
            Guard.IsNotEmpty(entityGroup, nameof(entityGroup));
            Guard.IsGreaterThan(languageId.Value, 0, nameof(languageId));

            var slug = await Table
                .ApplyActiveFilter()
                .FirstOrDefaultAsync(p => p.EntityId == entityId && p.EntityGroup == entityGroup && p.LanguageId == languageId);

            return slug;
        }

        public async Task<string> GetSlugAsync<T>(T entity, int? languageId = null)
            where T : BaseEntity, ISlugSupported
        {
            Guard.IsNotNull(entity, nameof(entity));

            var entityId = entity.Id;
            var entityGroup = entity.GetType().Name;

            languageId = languageId switch
            {
                0 => null,
                null => (await _workContext.GetWorkingLanguageAsync()).Id,
                _ => languageId
            };

            var slug = await GetActiveSlugAsync(entityId, entityGroup, languageId);

            return slug?.Value ?? string.Empty;
        }

        public async Task<int> GetSlugPerEntityAsync<T>(T entity, bool includeHidden = false) where T : BaseEntity, ISlugSupported
        {
            Guard.IsNotNull(entity, nameof(entity));

            var count = await Table
                .ApplyActiveFilter(includeHidden)
                .Where(p =>
                    p.EntityId == entity.Id &&
                    p.EntityGroup == typeof(T).Name)
                .CountAsync();

            return count;
        }

        public async Task SaveSlugAsync<T>(T entity, string value, int languageId = 0)
            where T : BaseEntity, ISlugSupported
        {
            Guard.IsNotNull(entity, nameof(entity));
            Guard.IsNotNullOrEmpty(value, nameof(value));
            Guard.IsGreaterThanOrEqualTo(languageId, 0, nameof(languageId));

            var entityId = entity.Id;
            var entityGroup = entity.GetType().Name;

            int? langId = languageId == 0 ? null : languageId;

            var slugs = await GetSlugsAsync(entity, langId, true, tracked: true);

            var activeSlug = slugs.FirstOrDefault(p => p.IsActive);

            if(activeSlug != null)
            {
                if (activeSlug.Value.Equals(value))
                    return;
                else
                    activeSlug.IsActive = false;
            }

            var slug = slugs.FirstOrDefault(p => p.Value == value);

            if(slug != null)
            {
                slug.IsActive = true;
            }
            else
            {
                slug = new Slug
                {
                    EntityId = entityId,
                    EntityGroup = entityGroup,
                    LanguageId = langId,
                    IsActive = true,
                    Value = value
                };

                await Table.AddAsync(slug);
            }

            using var transaction = await _context.Database.BeginTransactionAsync();

            await _context.SaveChangesAsync();

            await transaction.CommitAsync();
        }
        
        public async Task<string> ValidateSlugAsync<T>(T entity, string value) where T: BaseEntity, ISlugSupported
        {
            Guard.IsNotNull(entity, nameof(entity));
            Guard.IsNotNullOrEmpty(value, nameof(value));

            value = SeoHelper.ToSeoFriendly(value);

            async Task<bool> uniqueCheckAsync(string value)
            {
                var slug = await GetBySlugAsync(value);

                if (slug == null)
                    return true;

                if (slug.EntityId == entity.Id && slug.EntityGroup == typeof(T).Name)
                    return true;

                return false;
            }

            value = await SeoHelper.SeekUrlAsync(value, uniqueCheckAsync);

            return value;
        }

        public async Task DeleteSlugAsync<T>(T entity) where T: BaseEntity, ISlugSupported
        {
            Guard.IsNotNull(entity, nameof(entity));

            var slugs = await GetSlugsAsync(entity, true, true);

            if(slugs.Any())
            {
                using var transaction = await _context.Database.BeginTransactionAsync();

                foreach(var slug in slugs)
                    Table.Remove(slug);

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
            }
        }

        public async Task DeleteSlugAsync<T>(T entity, int? languageId = null)
            where T: BaseEntity, ISlugSupported
        {
            Guard.IsNotNull(entity, nameof(entity));
            Guard.IsGreaterThan(languageId.Value, 0, nameof(languageId));

            var slugs = await GetSlugsAsync(entity, languageId, true, true);

            if(slugs.Any())
            {
                using var transaction = await _context.Database.BeginTransactionAsync();

                foreach (var slug in slugs)
                    Table.Remove(slug);

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
            }
        }
    }
}
