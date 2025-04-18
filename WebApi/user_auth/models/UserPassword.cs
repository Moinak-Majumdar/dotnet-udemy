namespace user_auth.models
{
    public partial class UserPassword
    {
        public long UserId { get; set; }
        public byte[] PasswordHash { get; set; } = [0];
        public byte[] PasswordSalt { get; set; } = [0];
    }
}