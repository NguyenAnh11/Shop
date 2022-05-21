namespace Shop.Application.Users.Settings
{
    public class UserSetting : ISetting
    {
        public string PasswordHashAlgorithm { get; set; } = "SHA512";
        public int PasswordSaltSize { get; set; } = 6;
        public int PasswordFailedAllowAttempt { get; set; } = 5;
        public int PasswordFailedLockoutMinutes { get; set; } = 10;
        public int PasswordMinLength { get; set; } = 6;
        public int PasswordMaxLength { get; set; } = 30;
        public int PhoneMinLength { get; set; } = 10;
        public int PhoneMaxLength { get; set; } = 10;
        public RegisterType RegisterType { get; set; } = RegisterType.EmailValidation;
    }

    public enum RegisterType
    {
        Standard = 0,
        EmailValidation = 1
    }
}
