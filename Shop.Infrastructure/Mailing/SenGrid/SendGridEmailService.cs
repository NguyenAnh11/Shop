using Shop.Application.Mailing;

namespace Shop.Infrastructure.Mailing.SenGrid
{
    public class SendGridEmailService : IEmailService
    {
        public Task SendAsync(IList<string> toEmails, string subject, string body)
        {
            throw new NotImplementedException();
        }
    }
}
