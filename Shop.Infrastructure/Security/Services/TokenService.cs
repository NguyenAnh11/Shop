using Microsoft.IdentityModel.Tokens;
using Shop.Application.Security.Services;
using Shop.Infrastructure.Security.Configurations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Shop.Infrastructure.Security.Services
{
    public class TokenService : ITokenService
    {
        private readonly TokenConfig _tokenConfig;
        public TokenService()
        {
            _tokenConfig = Singleton<AppConfig>.Instance.Get<TokenConfig>();
        }

        protected string GetToken(List<Claim> claims, int tokenExpireInSecond)
        {
            Guard.IsNotNull(claims, nameof(claims));
            Guard.IsGreaterThan(tokenExpireInSecond, 0, nameof(tokenExpireInSecond));

            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenKey = Encoding.UTF8.GetBytes(_tokenConfig.Key);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Audience = _tokenConfig.Audience,
                Issuer = _tokenConfig.Issuer,
                IssuedAt = DateTime.Now,
                Expires = DateTime.Now.AddSeconds(tokenExpireInSecond),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var securityToken = tokenHandler.CreateToken(tokenDescriptor);

            var token = tokenHandler.WriteToken(securityToken);

            return token;
        }

        public string GetAccessToken(List<Claim> claims)
            => GetToken(claims, _tokenConfig.AccessTokenExpireInSecond);

        public string GetRefreshToken(List<Claim> claims)
            => GetToken(claims, _tokenConfig.RefreshTokenExpireInSecond);

        public ClaimsPrincipal VerifyToken(string token)
        {
            if (token.IsEmpty())
                return null;

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();

                var validationParameter = new TokenValidationParameters
                {
                    ValidateAudience = _tokenConfig.ValidateAudience,
                    ValidAudience = _tokenConfig.Audience,
                    ValidateIssuer = _tokenConfig.ValidateIssuer,
                    ValidIssuer = _tokenConfig.Issuer,
                    ValidateIssuerSigningKey = _tokenConfig.ValidateIssuerSigningKey,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenConfig.Key)),
                    ValidateLifetime = _tokenConfig.ValidateLifeTime
                };

                var principal = tokenHandler.ValidateToken(token, validationParameter, out SecurityToken securityToken);

                return principal;
            }
            catch
            {
                return null;
            }
        }

    }
}
