using Shop.Application.Users.Dtos;
using Shop.Domain.Users;

namespace Shop.Application.Users.Services
{
    public interface IUserService : IAbstractService<User>
    {
        Task<User> GetUserByEmailAsync(string email);

        Task<User> GetUserByIdAsync(int id, bool tracked = false);

        Task<User> GetUserByPhoneAsync(string phone);

        Task<bool> IsAdminAsync(User user);

        Task<bool> IsRegisterAsync(User user);

        Task<bool> IsVendorAsync(User user);

        Task<bool> IsGuestAsync(User user);

        Task<bool> IsRecoveryPasswordTokenValidAsync(string token);

        Task<Response<User>> VerifyUserSigninAsync(string email, string enterPassword);

        Task<Response<User>> ActiveAccountAsync(string token);

        Task<Response<User>> RegisterUserAsync(RegisterDto dto);

        Task<Response<string>> ChangePasswordAsync(User user, string oldPassword, string newPassword);
    }
}
