using Shop.Domain.Localization;
using Shop.SharedKernel;
using Shop.SharedKernel.Interfaces;

namespace Shop.Domain.Users
{
    public class User : BaseEntity, ISoftDelete, IClock
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int Gender { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public int? LanguageId { get; set; }
        public Language Language { get; set; }
        public string TimezoneId { get; set; }
        //public int? ShippingAddressId { get; set; }
        //public Address ShippingAddress { get; set; }
        //public int? BillingAddressId { get; set; }
        //public Address BillingAddress { get; set; }
        //public int? AddressId { get; set; }
        //public Address Address { get; set; }
        //public int? PictureId { get; set; }
        //public Picture Picture { get; set; }
        public int FailedLoginAttempt { get; set; }
        public DateTime? CannotLoginUntilUtc { get; set; }
        public DateTime? LastActivityUtc { get; set; }
        public DateTime CreateUtc { get; set; }
        public DateTime? UpdateUtc { get; set; }
        public IList<UserRole> Roles { get; set; } = new List<UserRole>();
    }
}
