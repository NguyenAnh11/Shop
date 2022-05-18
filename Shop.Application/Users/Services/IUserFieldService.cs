using Shop.Domain.Users;

namespace Shop.Application.Users.Services
{
    public interface IUserFieldService : IAbstractService<UserFied>
    {
        Task<TType> GetFieldAsync<TType>(User user, string field);

        Task SaveFieldAsync<TType>(User user, TType value, string field);
    }
}
