namespace Shop.Application.Catalog.Categories.Commands.Models
{
    public record UpdateCategoryCommand : AddCategoryCommand
    {
        public int Id { get; set; }
    }
}
