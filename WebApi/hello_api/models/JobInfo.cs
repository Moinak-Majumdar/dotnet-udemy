namespace hello_api.models
{
    public partial class JobInfo
    {
        public long UserId { get; set; }
        public string JobTitle { get; set; } = "";

        public string Department { get; set; } = "";

        public decimal Salary { get; set; }
    }
}