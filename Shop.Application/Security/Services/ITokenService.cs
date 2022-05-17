using System.Security.Claims;

namespace Shop.Application.Security.Services
{
    public interface ITokenService
    {
        string GetAccessToken(List<Claim> claims);

        string GetRefreshToken(List<Claim> claims);

        ClaimsPrincipal VerifyToken(string token);
    }
}
