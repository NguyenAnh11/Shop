using Shop.Domain.Localization;
using System.Linq.Expressions;

namespace Shop.Application.Localization.Services
{
    public class LocalizedEntityService : AbstractService<LocalizedEntity>, ILocalizedEntityService
    {
        private readonly IWorkContext _workContext;
        public LocalizedEntityService(ShopDbContext context) : base(context)
        {
        }

        private static string GetEntityKey<T>(Expression<Func<T, string>> func)
            => func?.Body switch
            {
                MemberExpression me => me.Member.Name,
                _ => throw new Exception()
            };

        private async Task<LocalizedEntity> GetLocalizedEntityAsync(int entityId, string entityGroup, string entityKey, int? languageId)
            => await Table
                .FirstOrDefaultAsync(p =>
                    p.EntityId == entityId &&
                    p.EntityGroup == entityGroup &&
                    p.EntityKey == entityKey &&
                    p.LanguageId == languageId);

        public async Task<string> GetLocalizedPropertyAsync<T>(T entity, string entityKey, int? languageId = null)
            where T : BaseEntity, ILocalizedEntity
        {
            Guard.IsNotNull(entity, nameof(entity));
            Guard.IsNotEmpty(entityKey, nameof(entityKey));

            var entityId = entity.Id;
            var entityGroup = entity.GetType().Name;

            languageId = languageId switch
            {
                0 => null,
                null => (await _workContext.GetWorkingLanguageAsync()).Id,
                _ => languageId
            };

            var lp = await GetLocalizedEntityAsync(entityId, entityGroup, entityKey, languageId);

            return lp?.EntityValue ?? string.Empty;
        }

        public async Task<string> GetLocalizedPropertyAsync<T>(T entity, Expression<Func<T, string>> func, int? languageId = null)
            where T : BaseEntity, ILocalizedEntity
            => await GetLocalizedPropertyAsync(entity, GetEntityKey(func), languageId);

        public async Task SaveLocalizedPropertyAsync<T>(T entity, string entityKey, string entityValue, int? languageId = null)
            where T: BaseEntity, ILocalizedEntity
        {
            Guard.IsNotNull(entity, nameof(entity));
            Guard.IsNotEmpty(entityKey, nameof(entityKey));

            var entityId = entity.Id;
            var entityGroup = entity.GetType().Name;

            languageId = languageId switch
            {
                0 => null,
                null => (await _workContext.GetWorkingLanguageAsync()).Id,
                _ => languageId
            };

            var lp = await GetLocalizedEntityAsync(entityId, entityGroup, entityKey, languageId);

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

                lp = new LocalizedEntity
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

        public async Task SaveLocalizePropertyAsync<T>(T entity, Expression<Func<T, string>> func, string entityValue, int? languageId = null)
            where T : BaseEntity, ILocalizedEntity
                => await SaveLocalizedPropertyAsync(entity, GetEntityKey(func), entityValue, languageId);   
    }
}
