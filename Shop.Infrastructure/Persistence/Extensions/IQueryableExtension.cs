using Shop.Application.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Shop.Infrastructure.Persistence.Extensions
{
    public static class IQueryableExtension
    {
        public static async Task<IPagedList<T>> ToPagedListAsync<T>(this IQueryable<T> source, int page, int pageSize)
        {
            var count = await source.CountAsync();
            var data = await source.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PagedList<T>(data, page, pageSize, count);
        }
    }
}
