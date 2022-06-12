using Shop.Domain.Catalog;
using Shop.Domain.Localization;

namespace Shop.Application.Catalog.Categories.Commands.Models
{
    public class DeleteCategoryTranslationCommand : IRequest
    {
        public Category Category { get; set; }
        public Language Language { get; set; }
        public DeleteCategoryTranslationCommand(Category category, Language language)
        {
            Category = category;
            Language = language;
        }
    }
}
