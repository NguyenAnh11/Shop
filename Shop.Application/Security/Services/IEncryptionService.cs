namespace Shop.Application.Security.Services
{
    public interface IEncryptionService
    {
        string CreateSalt();

        string CreateHash(string value, string salt);
    }
}
