using Shop.Domain.Users;

namespace Shop.Application.Authentication.Services
{
    public interface IAuthenticationService
    {
        Task<string> LoginAsync(User user);

        Task LogoutAsync();

        Task<Response<string>> RefreshTokenASync();
    }
}
