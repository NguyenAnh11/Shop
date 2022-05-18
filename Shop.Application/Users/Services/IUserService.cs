using Shop.Domain.Users;

namespace Shop.Application.Users.Services
{
    public interface IUserService : IAbstractService<User>
    {
        Task<User> GetUserByEmailAsync(string email);

        Task<User> GetUserByIdAsync(int id);

        Task<User> GetUserByPhoneAsync(string phone);

        Task<bool> IsAdminAsync(User user);

        Task<bool> IsRegisterAsync(User user);

        Task<bool> IsVendorAsync(User user);

        Task<bool> IsGuestAsync(User user);

        Task<Response<User>> ValidateUserAsync(string email, string enterPassword);
    }
}
