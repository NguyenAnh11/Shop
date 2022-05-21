namespace Shop.Application.Infrastructure.Services
{
    public interface IAbstractService<T> where T:BaseEntity
    {
        DbSet<T> Table { get; }
    }
}
