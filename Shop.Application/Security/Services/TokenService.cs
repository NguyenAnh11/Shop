using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Shop.Application.Security.Services
{
    public class TokenService : ITokenService
    {
        private readonly TokenConfig _tokenConfig;
        public TokenService()
        {
            _tokenConfig = Singleton<AppConfig>.Instance.Get<TokenConfig>();
        }

        public (string Token, DateTime Expires) GetToken(IList<Claim> claims, int expireInSecond)
        {
            Guard.IsNotNull(claims, nameof(claims));

            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenKey = Encoding.UTF8.GetBytes(_tokenConfig.Key);

            var now = DateTime.Now;
            var expires = now.AddSeconds(expireInSecond);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Audience = _tokenConfig.Audience,
                Issuer = _tokenConfig.Issuer,
                Expires = expires,
                IssuedAt = now,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var securityToken = tokenHandler.CreateToken(tokenDescriptor);

            var token = tokenHandler.WriteToken(securityToken);

            return (token, expires);
        }

        public string GetAccessToken(IList<Claim> claims)
            => GetToken(claims, _tokenConfig.AccessTokenExpireInSecond).Token;

        public (string Token, DateTime Expires) GetRefreshToken(IList<Claim> claims)
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
