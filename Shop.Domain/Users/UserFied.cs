using Shop.SharedKernel;

namespace Shop.Domain.Users
{
    public class UserFied : BaseEntity
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public string Field { get; set; }
        public string Value { get; set; }
    }
}
