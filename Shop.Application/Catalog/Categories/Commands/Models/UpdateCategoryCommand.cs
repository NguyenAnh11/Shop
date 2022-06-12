namespace Shop.Application.Catalog.Categories.Commands.Models
{
    public record UpdateCategoryCommand : CreateCategoryCommand
    {
        public int Id { get; set; }
    }
}
