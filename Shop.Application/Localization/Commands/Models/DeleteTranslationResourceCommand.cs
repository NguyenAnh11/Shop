using Shop.Domain.Localization;

namespace Shop.Application.Localization.Commands.Models
{
    public class DeleteTranslationResourceCommand : IRequest
    {
        public TranslationResource Translation { get; set; }

        public DeleteTranslationResourceCommand(TranslationResource translation)
        {
            Translation = translation;
        }
    }
}
