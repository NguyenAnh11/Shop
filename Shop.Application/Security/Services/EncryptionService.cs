using System.Security.Cryptography;
using System.Text;

namespace Shop.Application.Security.Services
{
    public class EncryptionService : IEncryptionService
    {
        public EncryptionService()
        {
            
        }

        public string CreateHash(string value, string salt, string algorithm)
        {
            Guard.IsNotEmpty(value, nameof(value));
            Guard.IsNotEmpty(salt, nameof(salt));

            var data = Encoding.UTF8.GetBytes(string.Concat(value, salt));
            var hash = (HashAlgorithm)CryptoConfig.CreateFromName(algorithm);

            Guard.IsNotNull(hash, nameof(hash));

            return BitConverter.ToString(hash.ComputeHash(data)).Replace("_", string.Empty);
        }

        public string CreateSalt(int size)
        {
            Guard.IsGreaterThan(size, 0, nameof(size));

            using var provider = RandomNumberGenerator.Create();
            var bytes = new byte[size];
            provider.GetBytes(bytes);

            return Convert.ToBase64String(bytes);
        }
    }
}
