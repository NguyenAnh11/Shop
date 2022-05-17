using Shop.Domain.Users;

namespace Shop.Application.Users.Services
{
    public interface IUserService : IAbstractService<User>
    {
        Task<User> GetUserByEmailAsync(string email);

        Task<User> GetUserByIdAsync(int id);

        Task<User> GetUserByPhoneAsync(string phone);

        Task<bool> VerifyPassword(User user, string enterPassword);

        Task<Response<User>> ValidateUserAsync(string email, string enterPassword);
    }
}
