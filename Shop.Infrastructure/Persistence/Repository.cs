using Microsoft.EntityFrameworkCore;
using Shop.Application.Infrastructure.Persistence;
using Shop.Infrastructure.Persistence.Context;
using Shop.SharedKernel;
using Shop.SharedKernel.Interfaces;

namespace Shop.Infrastructure.Persistence
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly ShopContext _context;
        public Repository(ShopContext context)
        {
            _context = context;
        }

        public IQueryable<T> Table => _context.Set<T>();

        private static IQueryable<T> ApplySoftDeleteFilter(IQueryable<T> source, bool includeDelete = false)
        {
            if (includeDelete)
                return source;

            if (!typeof(ISoftDelete).IsAssignableFrom(typeof(T)))
                return source;

            return source.OfType<ISoftDelete>().Where(p => !p.IsDelete).Cast<T>();
        }

        public async Task<T> GetByIdAsync(int id, bool includeDelete = false)
        {
            if (id == 0)
                return null;

            var query = ApplySoftDeleteFilter(Table, includeDelete);

            return await query.FirstOrDefaultAsync(p => p.Id == id);
        }

    }
}
