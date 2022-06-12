using Shop.Domain.Seo;

namespace Shop.Application.Seo.Services
{
    public interface ISlugService : IAbstractService<Slug>
    {
        
        Task<Slug> GetSlugByIdAsync(int id, bool tracked = false);

        Task<Slug> GetBySlugAsync(string value);

        Task<IList<Slug>> GetSlugsAsync<T>(T entity, bool includeHidden = false, bool tracked = false)
            where T : BaseEntity, ISlugSupported;

        Task<IList<Slug>> GetSlugsAsync<T>(T entity, int? languageId, bool includeHidden = false, bool tracked = false) 
            where T : BaseEntity, ISlugSupported;

        Task<Slug> GetActiveSlugAsync(int entityId, string entityGroup, int? languageId);

        Task<string> GetSlugAsync<T>(T entity, int? languageId = null) 
            where T : BaseEntity, ISlugSupported;

        Task<int> GetSlugPerEntityAsync<T>(T entity, bool includeHidden = false) 
            where T : BaseEntity, ISlugSupported;

        Task SaveSlugAsync<T>(T entity, string value, int languageId = 0) 
            where T : BaseEntity, ISlugSupported;

        Task<string> ValidateSlugAsync<T>(T entity, string value) 
            where T : BaseEntity, ISlugSupported;

        Task DeleteSlugAsync<T>(T entity) 
            where T : BaseEntity, ISlugSupported;

        Task InActiveSlugAsync<T>(T entity, int languageId)
            where T: BaseEntity, ISlugSupported;
    }
}
