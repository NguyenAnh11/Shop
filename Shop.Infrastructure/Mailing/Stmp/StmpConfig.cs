namespace Shop.Infrastructure.Mailing.Stmp
{
    public class StmpConfig : IConfig
    {
        public string Name => "Email:Stmp";
        public string Host { get; set; } = "smtp.gmail.com";
        public int Port { get; set; } = 465;
        public string Email { get; set; } = "anhnguyenviet256145@gmail.com";
        public string Password { get; set; } = "wtzccnsauoonuvsx";
    }
}
