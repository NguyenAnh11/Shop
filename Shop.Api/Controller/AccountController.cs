using Microsoft.AspNetCore.Mvc;
using Shop.Api.Models.Account;
using Shop.Domain.Users;
using System.Security.Claims;

namespace Shop.Api.Controller
{
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserSetting _userSetting;
        private readonly IWorkContext _workContext;
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        private readonly IUserFieldService _userFieldService;
        private readonly ILocalizationService _localizationService;
        private readonly IAuthenticationService _authenticationService;
        private readonly IMessageProviderService _messageProviderService;
        public AccountController(
            UserSetting userSetting,
            IWorkContext workContext,
            IUserService userService,
            ITokenService tokenService,
            IUserFieldService userFieldService,
            ILocalizationService localizationService,
            IAuthenticationService authenticationService,
            IMessageProviderService messageProviderService)
        {
            _userSetting = userSetting;
            _workContext = workContext;
            _userService = userService;
            _tokenService = tokenService;
            _messageProviderService = messageProviderService;
            _userFieldService = userFieldService;
            _localizationService = localizationService;
            _authenticationService = authenticationService;
        }

        [HttpPost]
        [Route("/account/login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var response = await _userService.VerifyUserSigninAsync(model.Email, model.Password);

            if (!response.Success)
            {
                return BadRequest(await _localizationService.GetResourceAsync(response.Message));
            }

            var token = await _authenticationService.LoginAsync(response.Data);

            return Ok(token);
        }

        [HttpPost]
        [Route("/account/register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var response = await _userService.RegisterUserAsync(new RegisterDto
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Phone = model.Phone,
                Gender = model.Gender,
                Password = model.Password
            });

            if (!response.Success)
            {
                return BadRequest(await _localizationService.GetResourceAsync(response.Message));
            }

            var language = await _workContext.GetWorkingLanguageAsync();

            switch (_userSetting.RegisterType)
            {
                case RegisterType.Standard:
                    await _messageProviderService.SendWelcomeMessageAsync(response.Data, language);
                    break;

                case RegisterType.EmailValidation:
                    List<Claim> claims = new()
                    {
                        new Claim(ClaimTypes.NameIdentifier, response.Data.Id.ToString())
                    };

                    var token = _tokenService.GetActiveAccountToken(claims);

                    await _userFieldService.SaveFieldAsync(response.Data, token, SystemUserFieldName.ActiveAccountToken);
                    var callbackUrl = Url.Action("ActiveAccount", "Account", new { token }, Url.ActionContext.HttpContext.Request.Scheme);

                    await _messageProviderService.SendActiveAccountMessageAsync(response.Data, language, callbackUrl);
                    break;
            }

            return Ok();
        }

        [HttpPost]
        [Route("/account/active")]
        public async Task<IActionResult> ActiveAccount([FromQuery] string token)
        {
            var response = await _userService.ActiveAccountAsync(token);

            if (!response.Success)
            {
                return BadRequest(await _localizationService.GetResourceAsync(response.Message));
            }

            await _messageProviderService.SendWelcomeMessageAsync(response.Data, await _workContext.GetWorkingLanguageAsync());

            return Ok(await _localizationService.GetResourceAsync(response.Message));
        }

        [HttpPost]
        [Route("/account/recovery-password")]
        public async Task<IActionResult> RecoveryPassword([FromBody] string email)
        {
            var user = await _userService.GetUserByEmailAsync(email);

            if (user == null || user.IsDelete || !user.IsActive)
            {
                return BadRequest(await _localizationService.GetResourceAsync("Account.RecoveryPassword.Error.EmailNotFound"));
            }

            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var token = _tokenService.GetRecoveryPasswordToken(claims);

            await _userFieldService.SaveFieldAsync(user, token, SystemUserFieldName.RecoveryPasswordToken);

            var callbackUrl = Url.Action("RecoveryPasswordConfirm", "Account", new { token }, Url.ActionContext.HttpContext.Request.Scheme);

            await _messageProviderService.SendRecoveryPasswordMessageAsync(user, await _workContext.GetWorkingLanguageAsync(), callbackUrl);

            return NoContent();
        }

        [HttpPost]
        [Route("/account/logout")]
        public async Task<IActionResult> Logout()
        {
            await _authenticationService.LogoutAsync();

            return Ok();
        }

        [HttpPost]
        [Route("/account/refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            var response = await _authenticationService.RefreshTokenASync();

            if (!response.Success)
            {
                return Unauthorized();
            }

            return Ok(response.Data);
        }
    }
}
