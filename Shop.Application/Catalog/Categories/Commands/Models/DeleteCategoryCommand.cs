using Shop.Domain.Catalog;

namespace Shop.Application.Catalog.Categories.Commands.Models
{
    public record DeleteCategoryCommand : IRequest
    {
        public Category Category { get; set; }

        public DeleteCategoryCommand(Category category)
        {
            Category = category;
        }
    }
}
