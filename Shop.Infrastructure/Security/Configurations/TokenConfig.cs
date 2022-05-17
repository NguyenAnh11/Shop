namespace Shop.Infrastructure.Security.Configurations
{
    public class TokenConfig : IConfig
    {
        public string Name => "Token";
        public string Key { get; set; }
        public string Audience => string.Join(';', Audiences);
        public string Issuer => string.Join(';', Issues);
        public IList<string> Audiences { get; set; } = new List<string>() { "http://localhost:3000" };
        public IList<string> Issues { get; set; } = new List<string>() { "http://localhost:5000" };
        public bool ValidateIssuer { get; set; } = true;
        public bool ValidateAudience { get; set; } = true;
        public bool ValidateIssuerSigningKey { get; set; } = true;
        public bool ValidateLifeTime { get; set; } = true;
        public int AccessTokenExpireInSecond { get; set; } = 3 * 60; //3 minute
        public int RefreshTokenExpireInSecond { get; set; } = 30 * 24 * 60 * 60; //30 day
        public int RecoveryPasswordTokenExpireInSecond { get; set; } = 15 * 60; //15 minute
        public int ActiveAccountTokenExpireInSecond { get; set; } = 15 * 60; //15 minute
    }
}
