using Shop.Application.Security.Services;
using Shop.Application.Users.Services;
using Shop.Domain.Users;
using System.Security.Claims;


namespace Shop.Application.Authentication.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ITokenService _tokenService;
        private readonly IUserService _userService;
        private readonly IUserFieldService _userFieldService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AuthenticationService(
            ITokenService tokenService,
            IUserService userService,
            IUserFieldService userFieldService,
            IHttpContextAccessor httpContextAccessor)
        {
            _tokenService = tokenService;
            _userService = userService;
            _userFieldService = userFieldService;
            _httpContextAccessor = httpContextAccessor;
        }

        protected static IList<Claim> GetClaimsFromUser(User user)
        {
            return new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };
        }

        protected void SaveTokenInCookie(string token, DateTime expires)
        {
            _httpContextAccessor.HttpContext.Response.Cookies.Append(token, "token", new CookieOptions
            {
                SameSite = SameSiteMode.Strict,
                Path = "/account/refresh-token",
                Expires = expires,
                HttpOnly = true,
            });
        }

        public async Task<string> SignInAsync(User user)
        {
            Guard.IsNotNull(user, nameof(user));

            var claims = GetClaimsFromUser(user);

            var accessToken = _tokenService.GetAccessToken(claims);

            var (refreshToken, expires) = _tokenService.GetRefreshToken(claims);

            SaveTokenInCookie(refreshToken, expires);

            await _userFieldService.SaveFieldAsync(user, refreshToken, SystemUserFieldName.RefreshToken);

            return accessToken;
        }

        public async Task SignOutAsync()
        {
            var princiapl = _httpContextAccessor.HttpContext.User;

            int id = int.Parse(princiapl.FindFirstValue(ClaimTypes.NameIdentifier));

            var user = await _userService.GetUserByIdAsync(id);

            await _userFieldService.SaveFieldAsync(user, SystemUserFieldName.RefreshToken, string.Empty);

            _httpContextAccessor.HttpContext.Response.Cookies.Delete("token");
        }

        public async Task<Response<string>> RefreshTokenASync()
        {
            var token = _httpContextAccessor.HttpContext.Request.Cookies["token"];

            if (token.IsEmpty())
                return Response<string>.Bad();

            var principal = _tokenService.VerifyToken(token);

            if (principal == null)
                return Response<string>.Bad();

            var user = await _userService.GetUserByIdAsync(int.Parse(principal.FindFirstValue(ClaimTypes.NameIdentifier)));

            if (user == null || user.IsDelete || !user.IsActive)
                return Response<string>.Bad();

            //get value refresh token store in database
            var savedToken = await _userFieldService.GetFieldAsync<string>(user, SystemUserFieldName.RefreshToken);

            if (savedToken.IsEmpty() || savedToken != token)
                return Response<string>.Bad();

            var claims = GetClaimsFromUser(user);

            var accessToken = _tokenService.GetAccessToken(claims);

            var (refreshToken, expires) = _tokenService.GetRefreshToken(claims);

            SaveTokenInCookie(refreshToken, expires);

            await _userFieldService.SaveFieldAsync(user, refreshToken, SystemUserFieldName.RefreshToken);

            return Response<string>.Ok(accessToken);
        }
    }
}
