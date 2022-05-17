using Shop.SharedKernel;

namespace Shop.Domain.Users
{
    public class UserPassword : BaseEntity
    {
        public string Hash { get; set; }
        public string Salt { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
