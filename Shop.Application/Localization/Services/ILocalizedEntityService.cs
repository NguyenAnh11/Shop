using Shop.Domain.Localization;
using System.Linq.Expressions;

namespace Shop.Application.Localization.Services
{
    public interface ILocalizedEntityService : IAbstractService<LocalizedEntity>
    {
        Task<string> GetLocalizedPropertyAsync<T>(T entity, Expression<Func<T, string>> func, int? languageId = null)
            where T : BaseEntity, ILocalizedEntity;

        Task SaveLocalizePropertyAsync<T>(T entity, Expression<Func<T, string>> func, string entityValue, int? languageId = null)
            where T : BaseEntity, ILocalizedEntity;

        
    }
}
