using System.Security.Claims;

namespace Shop.Application.Security.Services
{
    public interface ITokenService
    {
        protected (string Token, DateTime Expires) GetToken(IList<Claim> claims, int expireInSecond);

        string GetAccessToken(IList<Claim> claims);

        (string Token, DateTime Expires) GetRefreshToken(IList<Claim> claims);

        ClaimsPrincipal VerifyToken(string token);
    }
}
