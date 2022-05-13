using Shop.SharedKernel;

namespace Shop.Application.Infrastructure.Persistence
{
    public interface IRepository<T> where T: BaseEntity
    {
        IQueryable<T> Table { get; }
        Task<T> GetByIdAsync(int id, bool includeDelete = false);
    }
}
