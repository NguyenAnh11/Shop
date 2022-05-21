namespace Shop.Application.Infrastructure.Services
{
    public class AbstractService<T> : IAbstractService<T> where T : BaseEntity
    {
        protected readonly ShopDbContext _context;    
        public AbstractService(ShopDbContext context)
        {
            _context = context;
        }

        public DbSet<T> Table => _context.Set<T>();
    }
}
