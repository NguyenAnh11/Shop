namespace Shop.Application.Mailing
{
    public interface IEmailService
    {
        Task SendEmailAsync(IList<string> toEmails, string subject, string body);
    }
}
