using Shop.SharedKernel;
using Shop.SharedKernel.Interfaces;

namespace Shop.Domain.Customers
{
    public class Customer : BaseEntity, ISoftDelete, IClock
    {
        public bool IsDelete { get; set; }
        public DateTime CreateUtc { get; set; }
        public DateTime? UpdateUtc { get; set; }
    }
}
