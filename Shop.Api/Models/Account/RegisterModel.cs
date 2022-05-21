namespace Shop.Api.Models.Account
{
    public class RegisterModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int Gender { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
