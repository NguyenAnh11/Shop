namespace Shop.Application.Security.Services
{
    public interface IEncryptionService
    {
        string CreateSalt(int size);

        string CreateHash(string value, string salt, string algorithm);
    }
}
