namespace Shop.Application.Mailing
{
    public interface IEmailService
    {
        Task SendAsync(IList<string> toEmails, string subject, string body);
    }
}
