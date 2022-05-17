using Shop.Application.Security.Services;
using Shop.Infrastructure.Security.Configurations;
using System.Security.Cryptography;
using System.Text;

namespace Shop.Infrastructure.Security.Services
{
    public class EncryptionService : IEncryptionService
    {
        private readonly HashConfig _hashConfig;

        public EncryptionService()
        {
            _hashConfig = Singleton<AppConfig>.Instance.Get<HashConfig>();
        }

        public string CreateHash(string value, string salt)
        {
            Guard.IsNotEmpty(value, nameof(value));
            Guard.IsNotEmpty(salt, nameof(salt));

            var data = Encoding.UTF8.GetBytes(string.Concat(value, salt));
            var hash = (HashAlgorithm)CryptoConfig.CreateFromName(_hashConfig.HashAlgorithm);

            Guard.IsNotNull(hash, nameof(hash));

            return BitConverter.ToString(hash.ComputeHash(data)).Replace("_", string.Empty);
        }

        public string CreateSalt()
        {
            using var provider = RandomNumberGenerator.Create();
            var bytes = new byte[_hashConfig.SaltSize];
            provider.GetBytes(bytes);

            return Convert.ToBase64String(bytes);
        }
    }
}
