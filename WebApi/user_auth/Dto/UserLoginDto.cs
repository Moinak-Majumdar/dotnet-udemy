namespace user_auth.Dto
{
    public class UserLoginDto
    {
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";

    }

    public class UserLoginConfirmationDto
    {
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Email { get; set; } = "";
        public bool IsActive { get; set; }
        public bool IsPasswordSet { get; set; }
        public long UserId { get; set; }
        public byte[] PasswordHash { get; set; } = [0];
        public byte[] PasswordSalt { get; set; } = [0];
    }
}