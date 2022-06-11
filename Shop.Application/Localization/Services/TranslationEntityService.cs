using Shop.Domain.Localization;
using System.Linq.Expressions;

namespace Shop.Application.Localization.Services
{
    public class TranslationEntityService : AbstractService<TranslationEntity>, ITranslationEntityService
    {
        private readonly IWorkContext _workContext;
        public TranslationEntityService(ShopDbContext context) : base(context)
        {
        }

        private static string GetEntityKey<T>(Expression<Func<T, string>> func)
            => func?.Body switch
            {
                MemberExpression me => me.Member.Name,
                _ => throw new Exception()
            };

        private async Task<TranslationEntity> GetTranslationEntityAsync(int entityId, string entityGroup, string entityKey, int? languageId)
            => await Table
                .FirstOrDefaultAsync(p =>
                    p.EntityId == entityId &&
                    p.EntityGroup == entityGroup &&
                    p.EntityKey == entityKey &&
                    p.LanguageId == languageId);

        public async Task<string> GetTranslationPropertyAsync<T>(T entity, string entityKey, int? languageId = null)
            where T : BaseEntity, ITranslationEntity
        {
            Guard.IsNotNull(entity, nameof(entity));
            Guard.IsNotEmpty(entityKey, nameof(entityKey));
            Guard.IsGreaterThanOrEqualTo(languageId.Value, 0, nameof(languageId));

            var entityId = entity.Id;
            var entityGroup = entity.GetType().Name;

            languageId = languageId switch
            {
                0 => null,
                null => (await _workContext.GetWorkingLanguageAsync()).Id,
                _ => languageId
            };

            var lp = await GetTranslationEntityAsync(entityId, entityGroup, entityKey, languageId);

            return lp?.EntityValue ?? string.Empty;
        }

        public async Task<string> GetTranslationPropertyAsync<T>(T entity, Expression<Func<T, string>> func, int? languageId = null)
            where T : BaseEntity, ITranslationEntity
            => await GetTranslationPropertyAsync(entity, GetEntityKey(func), languageId);

        public async Task SaveTranslationPropertyAsync<T>(T entity, string entityKey, string entityValue, int? languageId = null)
            where T: BaseEntity, ITranslationEntity
        {
            Guard.IsNotNull(entity, nameof(entity));
            Guard.IsNotEmpty(entityKey, nameof(entityKey));
            Guard.IsGreaterThanOrEqualTo(languageId.Value, 0, nameof(languageId));

            var entityId = entity.Id;
            var entityGroup = entity.GetType().Name;

            languageId = languageId switch
            {
                0 => null,
                null => (await _workContext.GetWorkingLanguageAsync()).Id,
                _ => languageId
            };

            var lp = await GetTranslationEntityAsync(entityId, entityGroup, entityKey, languageId);

            if(lp != null)
            {
                if (entityValue.IsEmpty())
                    Table.Remove(lp);

                else if (!lp.EntityValue.Equals(entityValue))
                    lp.EntityValue = entityValue;

                else
                    return;
            }
            else
            {
                if (entityValue.IsEmpty())
                    return;

                lp = new TranslationEntity
                {
                    EntityId = entityId,
                    EntityGroup = entityGroup,
                    EntityKey = entityKey,
                    EntityValue = entityValue,
                    LanguageId = languageId
                };

                await Table.AddAsync(lp);
            }

            using var transaction = await _context.Database.BeginTransactionAsync();

            await _context.SaveChangesAsync();

            await transaction.CommitAsync();
        }

        public async Task SaveTranslationPropertyAsync<T>(T entity, Expression<Func<T, string>> func, string entityValue, int? languageId = null)
            where T : BaseEntity, ITranslationEntity
                => await SaveTranslationPropertyAsync(entity, GetEntityKey(func), entityValue, languageId);   

        public async Task DeleteTranslationEntityAsync<T>(T entity)
            where T: BaseEntity, ITranslationEntity
        {
            Guard.IsNotNull(entity, nameof(entity));

            var translations = await Table
                .Where(p => p.EntityId == entity.Id && p.EntityGroup == typeof(T).Name)
                .ToListAsync();

            using var transaction = await _context.Database.BeginTransactionAsync();

            Table.RemoveRange(translations);

            await _context.SaveChangesAsync();

            await transaction.CommitAsync();
        }

        public async Task DeleteAllTranslationPropertyAsync<T>(T entity, Expression<Func<T, string>> func)
            where T: BaseEntity, ITranslationEntity
        {
            Guard.IsNotNull(entity, nameof(entity));
            Guard.IsNotNull(func, nameof(func));

            var entityKey = GetEntityKey(func);

            var translations = await Table
                .Where(p => p.EntityId == entity.Id && p.EntityGroup == typeof(T).Name && p.EntityKey == entityKey)
                .ToListAsync();

            using var transaction = await _context.Database.BeginTransactionAsync();

            Table.RemoveRange(translations);

            await _context.SaveChangesAsync();

            await transaction.CommitAsync();
        }
    }
}
