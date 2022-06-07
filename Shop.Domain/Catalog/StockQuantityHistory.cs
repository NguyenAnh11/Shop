using Shop.SharedKernel;
using Shop.Domain.Users;

namespace Shop.Domain.Catalog
{
    public class StockQuantityHistory : BaseEntity
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int WareHouseId { get; set; }
        public WareHouse WareHouse { get; set; }
        public string Note { get; set; }
        public int AdjumentQuality { get; set; }
        public DateTime CreateUtc { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
