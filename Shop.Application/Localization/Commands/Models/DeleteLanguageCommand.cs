using Shop.Domain.Localization;

namespace Shop.Application.Localization.Commands.Models
{
    public class DeleteLanguageCommand : IRequest<Response>
    {
        public Language Language { get; set; }
        public DeleteLanguageCommand(Language language)
        {
            Language = language;
        }
    }
}
