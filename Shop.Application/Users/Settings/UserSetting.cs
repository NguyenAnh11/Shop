namespace Shop.Application.Users.Settings
{
    public class UserSetting : ISetting
    {
        public int FailedPasswordAllowAttempt { get; set; } = 5;
        public int FailedPasswordLockoutMinutes { get; set; } = 10;
        public int PasswordMinLength { get; set; } = 6;
        public int PasswordMaxLength { get; set; } = 30;
        public int PhoneMinLength { get; set; } = 10;
        public int PhoneMaxLength { get; set; } = 10;
    }
}
