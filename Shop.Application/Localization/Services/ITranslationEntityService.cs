using Shop.Domain.Localization;
using System.Linq.Expressions;

namespace Shop.Application.Localization.Services
{
    public interface ITranslationEntityService : IAbstractService<TranslationEntity>
    {
        Task<int> GetCountTranslationsPerEntityAsync<T>(T entity)
            where T : BaseEntity, ITranslationEntity;

        Task<string> GetTranslationPropertyAsync<T>(T entity, Expression<Func<T, string>> func, int? languageId = null)
            where T : BaseEntity, ITranslationEntity;

        Task SaveTranslationPropertyAsync<T>(T entity, Expression<Func<T, string>> func, string entityValue, int? languageId = null)
            where T : BaseEntity, ITranslationEntity;

        Task DeleteTranslationEntityAsync<T>(T entity)
            where T : BaseEntity, ITranslationEntity;

        Task DeleteAllTranslationPropertyAsync<T>(T entity, Expression<Func<T, string>> func)
            where T : BaseEntity, ITranslationEntity;

        Task DeleteTranslationEntityByLanguageAsync<T>(T entity, int? languageId = null)
            where T : BaseEntity, ITranslationEntity;
    }
}
