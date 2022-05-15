namespace Shop.Application.Infrastructure.Persistence
{
    public interface IRepository<T> where T : BaseEntity
    {
        IQueryable<T> Table { get; }
        Task<T> GetByIdAsync(int id, bool includeDelete = false);
        Task<IList<T>> GetListAsync(Func<IQueryable<T>, IQueryable<T>> func, bool includeDelete = false);
        Task<IList<T>> GetListAsync(Func<IQueryable<T>, Task<IQueryable<T>>> func, bool includeDelete = false);
        Task<IPagedList<T>> GetPagedListAsync(Func<IQueryable<T>, IQueryable<T>> func, int page, int pageSize, bool includeDelete = false);
        Task<IPagedList<T>> GetPagedListAsync(Func<IQueryable<T>, Task<IQueryable<T>>> func, int page, int pageSize, bool includeDelete = false);
        Task InsertAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<IDbContextTransaction> BeginTransactionAsync();
    }
}
