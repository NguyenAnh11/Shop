using Shop.SharedKernel;
using Shop.Domain.Media;

namespace Shop.Domain.Catalog
{
    public class ProductPicture : BaseEntity
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int PictureId { get; set; }
        public Picture Picture { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsDefault { get => DisplayOrder == 0; } 
    }
}
