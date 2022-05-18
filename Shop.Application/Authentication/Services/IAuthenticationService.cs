using Shop.Domain.Users;

namespace Shop.Application.Authentication
{
    public interface IAuthenticationService
    {
        Task<string> SignInAsync(User user);

        Task SignOutAsync();

        Task<Response<string>> RefreshTokenASync();
    }
}
