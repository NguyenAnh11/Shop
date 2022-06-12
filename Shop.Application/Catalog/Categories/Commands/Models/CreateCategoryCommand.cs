namespace Shop.Application.Catalog.Categories.Commands.Models
{
    public record CreateCategoryCommand : IRequest<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public bool IsActive { get; set; }
        public bool IsInlcudeTopMenu { get; set; }
        public bool IsShowOnHomePage { get; set; }
        public int PictureId { get; set; }
        public int ParentCategoryId { get; set; }
        public string Slug { get; set; }
        public string MetaTitle { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
    }
}
