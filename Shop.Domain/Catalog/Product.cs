using Shop.Domain.Seo;
using Shop.SharedKernel;
using Shop.SharedKernel.Interfaces;

namespace Shop.Domain.Catalog
{
    public class Product : BaseEntity, ISoftDelete, IClock, ISlugSupported, IActive
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public string Gtin { get; set; }
        public string SKu { get; set; }
        public bool IsDelete { get; set; }
        public bool IsActive { get; set; }
        public bool IsShowOnHomePage { get; set; }
        public bool IsTaxExempt { get; set; }
        public int? TaxId { get; set; }
        public bool IsFreeShipping { get; set; }
        public bool IsRecurring { get; set; }
        public bool IsMarkAsNew { get; set; }
        public bool IsAvaliableForOnline { get; set; }
        public DateTime? MarkAsNewStartDateUtc { get; set; }
        public DateTime? MarkAsNewEndDateUtc { get; set; }
        public int StockQuality { get; set; }
        public bool DisplayStockAvaliablity { get; set; }
        public bool DisplayStockQuality { get; set; }
        public int OrderMaxmimumQuality { get; set; }
        public int OrderMinimumQuality { get; set; }
        public bool IsAvaliableForOrder { get; set; }
        public bool IsDisableBuyButton { get; set; }
        public bool IsDisableWishlistButton { get; set; }
        public bool IsCallForPrice { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsAllowForReview { get; set; }
        public int ApprovedRatingSum { get; set; }
        public int ApprovedTotalReviews { get; set; }
        public decimal ApprovedRatingAverange => (decimal)ApprovedRatingSum / ApprovedTotalReviews;
        public int NotApprovedRatingSum { get; set; }
        public int NotApprovedTotalReviews { get; set; }
        public decimal NotApprovedRatingAverange => (decimal)NotApprovedRatingSum / NotApprovedTotalReviews;
        public decimal Price { get; set; }
        public decimal PriceOnline { get; set; }
        public decimal? Height { get; set; }
        public decimal? Width { get; set; }
        public decimal? Weight { get; set; }
        public int? ManufacturerId { get; set; } 
        public Manufacturer Manufacturer { get; set; }
        public DateTime? AvaliableStartDateUtc { get; set; }
        public DateTime? AvaliableEndDateUtc { get; set; }
        public DateTime CreateUtc { get; set; }
        public DateTime? UpdateUtc { get; set; }
        public string MetaTitle { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        public IList<ProductPicture> Pictures { get; set; } = new List<ProductPicture>();
        public IList<ProductCategory> Categories { get; set; } = new List<ProductCategory>();
        public IList<ProductVariantAttrbiute> ProductVariantAttrbiutes { get; set; } = new List<ProductVariantAttrbiute>();
        public IList<ProductSpecificationAttribute> ProductSpecificationAttributes { get; set; } = new List<ProductSpecificationAttribute>();
    }
}
