namespace hello_api.Dto
{
    public class AddUserDto
    {
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Email { get; set; } = "";
        public string Gender { get; set; } = "";
        public bool IsActive { get; set; }
    }
}