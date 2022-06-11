using Shop.Domain.Media;
using Shop.Domain.Seo;
using Shop.SharedKernel;
using Shop.SharedKernel.Interfaces;
using Shop.Domain.Localization;

namespace Shop.Domain.Catalog
{
    public class Category : BaseEntity, ISoftDelete, IClock, ISlugSupported, IActive, ITranslationEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public bool IsDelete { get; set; }
        public bool IsActive { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsInlcudeTopMenu { get; set; }
        public bool IsShowOnHomePage { get; set; }
        public int? PictureId { get; set; }
        public Picture Picture { get; set; }
        public int? ParentCategoryId { get; set; }
        public Category Parent { get; set; }    
        public DateTime CreateUtc { get; set; }
        public DateTime? UpdateUtc { get; set; }
        public string MetaTitle { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        public IList<Category> SubCategories { get; set; } = new List<Category>();
        public IList<ProductCategory> Products { get; set; } = new List<ProductCategory>();
    }
}
