namespace Shop.Application.Users.Dtos
{
    public class ChangePasswordDto
    {
        public ChangePasswordDto()
        {

        }

        public ChangePasswordDto(string oldPassword, string newPassword)
        {
            OldPassword = oldPassword;
            NewPassword = newPassword;
        }

        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
