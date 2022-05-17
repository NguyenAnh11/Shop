using Shop.Domain.Users;
using Shop.Application.Users.Settings;

namespace Shop.Application.Users.Services
{
    public class UserService : AbstractService<User>, IUserService
    {
        private readonly UserSetting _userSetting;
        private readonly IEncryptionService _encryptionService;
        public UserService(
            ShopDbContext context, 
            UserSetting userSetting,
            IEncryptionService encryptionService) : base(context)
        {
            _userSetting = userSetting;
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

        public async Task<User> GetUserByIdAsync(int id)
            => await Table.FindByIdAsync(id);

        public async Task<User> GetUserByPhoneAsync(string phone)
        {
            if (phone.IsEmpty())
                return null;

            var user = await Table
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Phone == phone);

            return user;
        }

        public async Task<bool> VerifyPassword(User user, string enterPassword)
        {
            var userPassword = await _context
                .Set<UserPassword>()
                .FirstOrDefaultAsync(p => p.UserId == user.Id);

            var password = _encryptionService.CreateHash(enterPassword, userPassword.Salt);

            return userPassword.Hash == password;
        }

        public async Task<Response<User>> ValidateUserAsync(string email, string enterPassword)
        {
            var user = await GetUserByEmailAsync(email);

            if (user == null || user.IsDelete)
                return Response<User>.Bad("Login.Error.NotFound");

            if (!user.IsActive)
                return Response<User>.Bad("Login.Error.NotActive");

            if (user.CannotLoginUntilUtc.HasValue && user.CannotLoginUntilUtc > DateTime.UtcNow)
                return Response<User>.Bad("Login.Error.Lockedout");

            if(!await VerifyPassword(user, enterPassword))
            {
                user.FailedLoginAttempt += 1;

                if(_userSetting.FailedPasswordAllowAttempt > 0 && user.FailedLoginAttempt > _userSetting.FailedPasswordAllowAttempt)
                {
                    user.FailedLoginAttempt = 0;
                    user.CannotLoginUntilUtc = DateTime.UtcNow.AddMinutes(_userSetting.FailedPasswordLockoutMinutes);
                }

                await _context.SaveChangesAsync();

                return Response<User>.Bad("Login.Error.WrongCredentical");
            }

            user.FailedLoginAttempt = 0;
            user.CannotLoginUntilUtc = null;
            user.LastActivityUtc = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Response<User>.Ok(user);
        }

        //public async Task
    }
}
