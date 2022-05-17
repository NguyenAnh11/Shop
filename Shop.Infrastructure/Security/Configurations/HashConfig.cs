namespace Shop.Infrastructure.Security.Configurations
{
    public class HashConfig : IConfig
    {
        public string Name => "Hash";
        public int SaltSize { get; set; } = 10;
        public string HashAlgorithm { get; set; } = "SHA512";
    }
}
