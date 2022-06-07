namespace Shop.Application.Mailing
{
    public class EmailSender
    {
        private readonly IEmailService _emailService;
        public EmailSender(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public async Task SendEmailAsync(IList<string> toEmails, string subject, string body)
            => await _emailService.SendAsync(toEmails, subject, body);
    }
}
