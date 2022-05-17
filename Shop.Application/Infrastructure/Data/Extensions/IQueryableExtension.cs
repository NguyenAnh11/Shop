using System.Linq.Expressions;
using Shop.SharedKernel.Interfaces;

namespace Shop.Application.Infrastructure.Data.Extensions
{
    public static class IQueryableExtension
    {
        public static IQueryable<T> ApplyTracking<T>(this IQueryable<T> source, bool tracked = false) where T : BaseEntity
            => tracked ? source : source.AsNoTracking();

        public static IQueryable<T> ApplyDeletedFilter<T>(this IQueryable<T> source, bool includeDeleted = false) where T : BaseEntity
            => includeDeleted ? source : source.IgnoreQueryFilters();

        public static IQueryable<T> ApplyActiveFilter<T>(this IQueryable<T> source, bool includeHidden = false)
            where T : class, IActive
            => includeHidden ? source : source.Where(p => p.IsActive);

        public static IQueryable<T> ApplyPatternFilter<T>(this IQueryable<T> query, Expression<Func<T, bool>> predicate)
        {
            Guard.IsNotNull(query, nameof(query));
            Guard.IsNotNull(predicate, nameof(predicate));

            return query.Where(predicate);
        }

        public static async Task<IPagedList<T>> ToPagedListAsync<T>(this IQueryable<T> source, int page, int pageSize) where T : BaseEntity   
        {
            page = Math.Min(page, 1);
            pageSize = Math.Min(pageSize, 1);

            int count = await source.CountAsync();
            var data = await source.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PagedList<T>(data, page, pageSize, count);
        }
    }
}
