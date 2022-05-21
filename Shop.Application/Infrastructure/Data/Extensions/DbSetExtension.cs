namespace Shop.Application.Infrastructure.Data.Extensions
{
    public static class DbSetExtension
    {

        public static async Task<T> FindByIdAsync<T>(this DbSet<T> source, int id, bool includeDeleted = false, bool tracked = false) where T: BaseEntity
        {
            if(id == 0)
                return null;

            var model = await source
                .ApplyTracking(tracked)
                .ApplyDeletedFilter(includeDeleted)
                .FirstOrDefaultAsync(p => p.Id == id);

            return model;
        }

        public static async Task<IList<T>> FindByIdsAsync<T>(this DbSet<T> source, List<int> ids, bool includeDeleted = false, bool tracked = false) where T: BaseEntity
        {
            Guard.IsNotNull(ids, nameof(ids));

            if (!ids.Any())
                return null;

            if (ids.Contains(0))
                ids.RemoveAll(p => p == 0);

            var model = await source
                .ApplyTracking(tracked)
                .ApplyDeletedFilter(includeDeleted)
                .Where(p => ids.Contains(p.Id))
                .ToListAsync();

            return model;
        }
    }
}
