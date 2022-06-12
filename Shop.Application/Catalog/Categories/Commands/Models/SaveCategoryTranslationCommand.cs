using Shop.Domain.Localization;
using Shop.Domain.Catalog;

namespace Shop.Application.Catalog.Categories.Commands.Models
{
    public class SaveCategoryTranslationCommand : IRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public string MetaTitle { get; set; }   
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        public string Slug { get; set; }
        public Language Language { get; set; }
        public Category Category { get; set; }
    }
}
