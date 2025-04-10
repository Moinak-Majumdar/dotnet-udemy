namespace hello_api.models
{
    public partial class User
    {
        public long UserId { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Email { get; set; } = "";
        public string Gender { get; set; } = "";
        public bool IsActive { get; set; }
    }
}