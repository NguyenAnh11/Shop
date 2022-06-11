using Shop.Domain.Seo;
using Shop.SharedKernel;
using Shop.SharedKernel.Interfaces;
using Shop.Domain.Media;

namespace Shop.Domain.Catalog
{
    public class Manufacturer: BaseEntity, ISoftDelete, IClock, ISlugSupported, IActive
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public bool IsDelete { get; set; }
        public bool IsActive { get; set; }
        public int? PictureId { get; set; }
        public Picture Picture { get; set; }
        public DateTime CreateUtc { get; set; }
        public DateTime? UpdateUtc { get; set; }
        public string MetaTitle { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        public IList<Product> Products { get; set; } = new List<Product>();
    }
}