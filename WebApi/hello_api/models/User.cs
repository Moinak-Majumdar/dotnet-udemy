namespace hello_api
{
    public partial class User
    {
        public long UserId { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Email { get; set; } = "";
        public string Gender { get; set; } = "";
        public string JobTitle { get; set; } = "";
        public string Department { get; set; } = "";
        public bool IsActive { get; set; }
        public decimal Salary { get; set; }
    }
}