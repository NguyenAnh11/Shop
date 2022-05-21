using Shop.Application.Security.Services;
using Shop.Application.Users.Dtos;
using Shop.Application.Users.Settings;
using Shop.Domain.Users;
using System.Security.Claims;

namespace Shop.Application.Users.Services
{
    public class UserService : AbstractService<User>, IUserService
    {
        private readonly UserSetting _userSetting;
        private readonly IRoleService _roleService;
        private readonly ITokenService _tokenService;
        private readonly IUserFieldService _userFieldService;
        private readonly IEncryptionService _encryptionService;
        public UserService(
            ShopDbContext context,
            UserSetting userSetting,
            IRoleService roleService,
            ITokenService tokenService,
            IUserFieldService userFieldService,
            IEncryptionService encryptionService) : base(context)
        {
            _userSetting = userSetting;
            _roleService = roleService;
            _tokenService = tokenService;
            _userFieldService = userFieldService;
            _encryptionService = encryptionService;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            if (email.IsEmpty())
                return null;

            var user = await Table
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Email == email);

            return user;
        }

        public async Task<User> GetUserByIdAsync(int id, bool tracked = false)
            => await Table.FindByIdAsync(id, tracked: tracked);

        public async Task<User> GetUserByPhoneAsync(string phone)
        {
            if (phone.IsEmpty())
                return null;

            var user = await Table
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Phone == phone);

            return user;
        }

        public async Task<bool> IsAdminAsync(User user)
            => await _roleService.IsInRoleAsync(user, SystemRoleName.Adminstrator);

        public async Task<bool> IsRegisterAsync(User user)
            => await _roleService.IsInRoleAsync(user, SystemRoleName.Register);

        public async Task<bool> IsVendorAsync(User user)
            => await _roleService.IsInRoleAsync(user, SystemRoleName.Vendor);

        public async Task<bool> IsGuestAsync(User user)
            => await _roleService.IsInRoleAsync(user, SystemRoleName.Guest);

        protected async Task<UserPassword> GetUserPasswordAsync(User user)
        {
            Guard.IsNotNull(user, nameof(user));

            return await _context
                .Set<UserPassword>()
                .FirstOrDefaultAsync(p => p.UserId == user.Id);
        }

        protected bool VerifyPassword(UserPassword userPassword, string enterPassword)
        {
            var password = _encryptionService.CreateHash(enterPassword, userPassword.Salt, _userSetting.PasswordHashAlgorithm);

            return userPassword.Hash == password;
        }

        public async Task<bool> IsRecoveryPasswordTokenValidAsync(string token)
        {
            if (token.IsEmpty())
                return false;

            var principal = _tokenService.VerifyToken(token);

            if (principal == null)
                return false;

            var id = int.Parse(principal.FindFirstValue(ClaimTypes.NameIdentifier));

            var user = await GetUserByIdAsync(id);

            if (user == null || !user.IsActive || user.IsDelete)
                return false;

            var savedToken = await _userFieldService.GetFieldAsync<string>(user, SystemUserFieldName.RecoveryPasswordToken);

            if (savedToken.IsEmpty() || savedToken != token)
                return false;

            return true;
        }

        public async Task<Response<User>> VerifyUserSigninAsync(string email, string enterPassword)
        {
            var user = await GetUserByEmailAsync(email);

            if (user == null || user.IsDelete)
                return Response<User>.Bad("Account.Login.Error.NotFound");

            if (!user.IsActive)
                return Response<User>.Bad("Account.Login.Error.NotActive");

            if (user.CannotLoginUntilUtc.HasValue && user.CannotLoginUntilUtc > DateTime.UtcNow)
                return Response<User>.Bad("Account.Login.Error.Lockedout");

            if (!VerifyPassword(await GetUserPasswordAsync(user), enterPassword))
            {
                user.FailedLoginAttempt += 1;

                if (_userSetting.PasswordFailedAllowAttempt > 0 && user.FailedLoginAttempt > _userSetting.PasswordFailedAllowAttempt)
                {
                    user.FailedLoginAttempt = 0;
                    user.CannotLoginUntilUtc = DateTime.UtcNow.AddMinutes(_userSetting.PasswordFailedLockoutMinutes);
                }

                await _context.SaveChangesAsync();

                return Response<User>.Bad("Account.Login.Error.WrongCredentical");
            }

            user.FailedLoginAttempt = 0;
            user.CannotLoginUntilUtc = null;
            user.LastActivityUtc = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Response<User>.Ok(user);
        }

        public async Task<Response<User>> ActiveAccountAsync(string token)
        {
            if (token.IsEmpty())
                return Response<User>.Bad();

            var principal = _tokenService.VerifyToken(token);

            if (principal == null)
                return Response<User>.Bad("Account.Active.Error.InvalidLink");

            var user = await GetUserByIdAsync(int.Parse(principal.FindFirstValue(ClaimTypes.NameIdentifier)), true);

            if (user == null || user.IsDelete)
                return Response<User>.Bad();

            if (user.IsActive)
                return Response<User>.Ok("Account.Active.Ok.AlreadyActive");

            var savedToken = await _userFieldService.GetFieldAsync<string>(user, SystemUserFieldName.ActiveAccountToken);

            if (savedToken != null && savedToken != token)
                return Response<User>.Bad();

            using var transaction = await _context.Database.BeginTransactionAsync();

            user.IsActive = true;

            await _userFieldService.SaveFieldAsync(user, string.Empty, SystemUserFieldName.ActiveAccountToken);

            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

            return Response<User>.Ok(user, "Account.Active.Ok");
        }

        public async Task<Response<string>> ChangePasswordAsync(User user, string oldPassword, string newPassword)
        {
            Guard.IsNotNull(user, nameof(user));
            Guard.IsNotNullOrEmpty(oldPassword, nameof(oldPassword));
            Guard.IsNotNullOrEmpty(newPassword, nameof(newPassword));

            var userPassword = await GetUserPasswordAsync(user);

            if (!VerifyPassword(userPassword, oldPassword))
            {
                return Response<string>.Bad("ChangePassword.Error.OldPasswordNotMatch");
            }

            _context.Set<UserPassword>().Remove(userPassword);

            var salt = _encryptionService.CreateSalt(_userSetting.PasswordSaltSize);
            var hash = _encryptionService.CreateHash(newPassword, salt, _userSetting.PasswordHashAlgorithm);

            await _context
                .Set<UserPassword>()
                .AddAsync(new UserPassword
                {
                    UserId = user.Id,
                    Salt = salt,
                    Hash = hash
                });

            await _context.SaveChangesAsync();

            return Response<string>.Ok();
        }

        public async Task<Response<User>> RegisterUserAsync(RegisterDto dto)
        {
            Guard.IsNotNull(dto, nameof(dto));

            var user = await GetUserByEmailAsync(dto.Email);

            if (user != null)
            {
                return Response<User>.Bad("Register.Error.EmailAlreadyUsed");
            }

            user = await GetUserByPhoneAsync(dto.Phone);

            if (user != null)
            {
                return Response<User>.Bad("Register.Error.PhoneAlreadyUsed");
            }

            using var transaction = await _context.Database.BeginTransactionAsync();

            user = new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Phone = dto.Phone,
                Gender = dto.Gender,
            };

            switch (_userSetting.RegisterType)
            {
                case RegisterType.Standard:
                    user.IsActive = true;
                    break;

                case RegisterType.EmailValidation:
                    user.IsActive = false;
                    break;
            }

            await Table.AddAsync(user);

            var salt = _encryptionService.CreateSalt(_userSetting.PasswordSaltSize);
            var hash = _encryptionService.CreateHash(dto.Password, salt, _userSetting.PasswordHashAlgorithm);

            await _context.Set<UserPassword>().AddAsync(new UserPassword
            {
                User = user,
                Salt = salt,
                Hash = hash,
            });

            var role = await _roleService.GetRoleByNameAsync(SystemRoleName.Register);

            Guard.IsNotNull(role, nameof(role));

            await _context.Set<UserRole>().AddAsync(new UserRole { User = user, RoleId = role.Id });

            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

            return Response<User>.Ok(user);
        }
    }
}
