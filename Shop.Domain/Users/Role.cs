using Shop.SharedKernel;
using Shop.SharedKernel.Interfaces;

namespace Shop.Domain.Users
{
    public class Role : BaseEntity, IClock, IActive
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsFreeShipping { get; set; }
        public bool IsTaxExempt { get; set; }
        public bool IsActive { get; set; }
        public bool IsSystemRole { get; set; }
        public int DisplayOrder { get; set; }
        public DateTime CreateUtc { get; set; }
        public DateTime? UpdateUtc { get; set; }
        public IList<UserRole> Users { get; set; } = new List<UserRole>();
    }
}
