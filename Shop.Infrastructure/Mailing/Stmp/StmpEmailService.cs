using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Shop.Application.Mailing;

namespace Shop.Infrastructure.Mailing.Stmp
{
    public class StmpEmailService : IEmailService
    {
        private readonly StmpConfig _stmpConfig;
        public StmpEmailService()
        {
            _stmpConfig = Singleton<AppConfig>.Instance.Get<StmpConfig>();
        }

        public async Task SendEmailAsync(IList<string> toEmails, string subject, string body)
        {
            Guard.IsNotNullOrEmpty(body, nameof(body));
            Guard.IsNotNull(toEmails, nameof(toEmails));
            Guard.IsNotNullOrEmpty(subject, nameof(subject));

            if (!toEmails.Any())
                return;

            var email = new MimeMessage
            {
                Sender = MailboxAddress.Parse(_stmpConfig.Email),
            };

            email.To.AddRange(toEmails.Select(p => MailboxAddress.Parse(p)));

            email.Subject = subject;

            var builder = new BodyBuilder()
            {
                HtmlBody = body
            };

            email.Body = builder.ToMessageBody();

            using var client = new SmtpClient();
            client.ServerCertificateValidationCallback = (s, c, h, e) => true;

            await client.ConnectAsync(_stmpConfig.Host, _stmpConfig.Port, SecureSocketOptions.SslOnConnect);
            client.AuthenticationMechanisms.Remove("XOAUTH2");

            await client.AuthenticateAsync(_stmpConfig.Email, _stmpConfig.Password); ;
            await client.SendAsync(email);

            await client.DisconnectAsync(true);
        }
    }
}
