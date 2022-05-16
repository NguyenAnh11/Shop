using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Toolkit.Diagnostics;
using Shop.Application.Infrastructure.Models;
using Shop.Application.Infrastructure.Persistence;
using Shop.Infrastructure.Persistence.Context;
using Shop.Infrastructure.Persistence.Extensions;
using Shop.SharedKernel;

namespace Shop.Infrastructure.Persistence
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly ShopContext _context;

        public Repository(ShopContext context)
        {
            _context = context;
        }

        public IQueryable<T> Table => _context.Set<T>().AsNoTracking();

        private static IQueryable<T> ApplyDeleteFilter(IQueryable<T> source, bool includeDelete = false)
            => includeDelete ? source : source.IgnoreQueryFilters();

        public async Task<T> GetByIdAsync(int id, bool includeDelete = false)
        {
            if (id == 0) return null;

            var query = ApplyDeleteFilter(Table, includeDelete);

            return await query.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IList<T>> GetListAsync(Func<IQueryable<T>, IQueryable<T>> func, bool includeDelete = false)
        {
            Guard.IsNotNull(func, nameof(func));

            var query = ApplyDeleteFilter(Table, includeDelete);

            return await func(query).ToListAsync();
        }

        public async Task<IList<T>> GetListAsync(Func<IQueryable<T>, Task<IQueryable<T>>> func, bool includeDelete = false)
        {
            Guard.IsNotNull(func, nameof(func));

            var query = await func(ApplyDeleteFilter(Table, includeDelete));

            return await query.ToListAsync();
        }

        public async Task<IPagedList<T>> GetPagedListAsync(Func<IQueryable<T>, IQueryable<T>> func, int page, int pageSize, bool includeDelete = false)
        {
            Guard.IsNotNull(func, nameof(func));

            page = Math.Min(page, 1);
            pageSize = Math.Min(pageSize, 1);

            var query = ApplyDeleteFilter(Table, includeDelete);

            return await func(query).ToPagedListAsync(page, pageSize);
        }

        public async Task<IPagedList<T>> GetPagedListAsync(Func<IQueryable<T>, Task<IQueryable<T>>> func, int page, int pageSize, bool includeDelete = false)
        {
            Guard.IsNotNull(func, nameof(func));

            page = Math.Min(page, 1);
            pageSize = Math.Min(pageSize, 1);

            var query = await func(ApplyDeleteFilter(Table, includeDelete));

            return await query.ToPagedListAsync(page, pageSize);
        }

        public async Task InsertAsync(T entity)
        {
            Guard.IsNotNull(entity, nameof(entity));

            await _context.Set<T>().AddAsync(entity);

            await _context.SaveChangesAsync();
        }

        public async Task Update(T entity)
        {
            Guard.IsNotNull(entity, nameof(entity));

            _context.Set<T>().Update(entity);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            Guard.IsNotNull(entity, nameof(entity));

            _context.Set<T>().Remove(entity);

            await _context.SaveChangesAsync();
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
            => await _context.Database.BeginTransactionAsync();
    }
}
